namespace SapAutomation.Managers.GitHubTutorialManager
{
    using AemTagManager;
    using AemUserManager;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.ApiManager;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.GitManager;
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using Web.Pages.Sap.TutorialCatalogPage;

    [CommandManager(typeof(GitHubTutorialManagerConfig), "Manager for tutorial")]
    public class GitHubTutorialManager : BaseCommandManager
    {
        private class LocalContainer
        {
            public string tempDir { get; set; }
        }

        ThreadLocal<LocalContainer> _container;

        public GitHubTutorialManager(GitHubTutorialManagerConfig config)
        {
            _container = new ThreadLocal<LocalContainer>(() =>
            {
                var localContainer = new LocalContainer();
                localContainer.tempDir = config.TempFolderPath;

                if (!Directory.Exists(localContainer.tempDir))
                    Directory.CreateDirectory(localContainer.tempDir);

                return localContainer;
            });
        }

        [Command("Generate GitHub tutorial files")]
        public void GenerateTutorial(GitHubTutorial tutorial, ILogger log)
        {
            log?.INFO($"Generate GitHub tutorial files for tutorial: {tutorial.UniqueName}");
            try
            {
                var tutorialPath = Path.Combine(_container.Value.tempDir, DateTime.UtcNow.ToFileTimeUtc().ToString(), tutorial.Folder);
                log?.DEBUG($"Local tutorial folder: {tutorialPath}");

                if (!Directory.Exists(tutorialPath))
                    Directory.CreateDirectory(tutorialPath);

                foreach (var tutorialTest in tutorial.GitHubTutorialTests)
                {
                    var tutorialFile = tutorialTest.TutorialFile;
                    log?.DEBUG($"Generate GitHub tutorial file: {tutorialFile.Folder}\\{tutorialFile.Name}");

                    var tutorialItemPath = Path.Combine(tutorialPath, tutorialFile.Folder);
                    log?.TRACE($"GitHub tutorial file folder: {tutorialItemPath}");

                    if (!Directory.Exists(tutorialItemPath))
                        Directory.CreateDirectory(tutorialItemPath);

                    var sb = new StringBuilder();

                    sb.AppendLine("---");

                    if (tutorialFile.Title != null)
                        sb.AppendLine($"title: {tutorialFile.Title}");

                    if (tutorialFile.Description != null)
                        sb.AppendLine($"description: {tutorialFile.Description}");

                    if (tutorialFile.Tags != null)
                    {
                        var listTags = new StringBuilder();
                        var isFirst = true;
                        foreach (var aemTag in tutorialFile.Tags)
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                                listTags.Append(aemTag);
                            }
                            else
                            {
                                listTags.Append(", ");
                                listTags.Append(aemTag);
                            }
                        }

                        sb.AppendLine($"tags: [{listTags}]");
                    }

                    sb.AppendLine("---");
                    sb.AppendLine();

                    if (tutorialFile.Content != null)
                        sb.AppendLine(tutorialFile.Content);

                    log?.TRACE($"GitHub tutorial file: {tutorialFile.Name} content:\n{sb.ToString()}");

                    string file = Path.Combine(tutorialItemPath, tutorialFile.Name + ".md");
                    File.WriteAllText(file, sb.ToString());

                    log?.TRACE($"Generate GitHub tutorial file: {tutorialFile.Folder}\\{tutorialFile.Name} successfully completed");
                }

                tutorial.PathToGeneratedTutorial = tutorialPath;
                log?.INFO($"Generate GitHub tutorial files for tutorial: {tutorial.UniqueName} successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during generating tutorial: {tutorial.UniqueName}");
                throw new CommandAbortException($"Error occurred during generating tutorial: {tutorial.UniqueName}", ex);
            }
        }

        [Command("Copy tutorial to specified directory if files are newer", "CopyToGitRepository")]
        public void CopyToGitRepository(GitHubTutorial tutorial, GitRepositoryConfig repositoryConfig, ILogger log)
        {
            log?.INFO($"Copying files from: {tutorial.PathToGeneratedTutorial ?? "null"} to: {repositoryConfig.LocalRepository}");

            try
            {
                if (tutorial.PathToGeneratedTutorial == null || !Directory.Exists(tutorial.PathToGeneratedTutorial))
                {
                    log?.ERROR($"Tutorial: {tutorial.UniqueName} hasn't generated yet. Possible path: {tutorial.PathToGeneratedTutorial ?? "null"}");
                    throw new CommandAbortException($"Tutorial: {tutorial.UniqueName} hasn't generated yet. Possible path: {tutorial.PathToGeneratedTutorial ?? "null"}");
                }

                var di = new DirectoryInfo(tutorial.PathToGeneratedTutorial);
                var files = di.GetFiles("*.*", SearchOption.AllDirectories).ToList();
                log?.DEBUG($"Found: {files.Count} files to copying");

                var targetDir = new DirectoryInfo(repositoryConfig.LocalRepository);

                var existedFileInfos = targetDir.GetFiles("*.*", SearchOption.AllDirectories);
                var existedFiles = new List<string>();
                for (int i = 0; i < existedFileInfos.Length; i++)
                    existedFiles.Add(existedFileInfos[i].FullName.Replace(targetDir.FullName + "\\", ""));

                foreach (var file in files)
                {
                    log?.DEBUG($"Copying file: {file.FullName}");
                    var relativePath = file.FullName.Replace(di.Parent.FullName + "\\", "");
                    log?.TRACE($"Relative file path: {relativePath}");
                    var newPath = targetDir.FullName + "\\" + relativePath;
                    log?.TRACE($"Destination path: {newPath}");

                    var needToCopy = true;
                    if (File.Exists(newPath))
                    {
                        needToCopy = false;

                        log?.TRACE("Destination file already exists. Compare contents");
                        var newLines = File.ReadAllLines(file.FullName);
                        log?.TRACE($"New file contains: {newLines.Length} lines");
                        var oldLines = File.ReadAllLines(newPath);
                        log?.TRACE($"Old file contains: {oldLines.Length} lines");

                        if (newLines.Length != oldLines.Length)
                        {
                            log?.TRACE($"Files have different count of lines. So destination file will be replaced");
                            needToCopy = true;
                        }
                        else
                        {
                            for (int i = 0; i < newLines.Length; i++)
                            {
                                if (newLines[i] != oldLines[i])
                                {
                                    log?.TRACE($"Files are different in line № {i + 1}. So destination file will be replaced");
                                    needToCopy = true;
                                    break;
                                }
                            }
                        }
                    }

                    existedFiles.Remove(relativePath);

                    if (needToCopy)
                    {
                        var newDirectory = Path.GetDirectoryName(newPath);
                        if (!Directory.Exists(newDirectory))
                            Directory.CreateDirectory(newDirectory);
                        File.Copy(file.FullName, newPath, true);
                        log?.TRACE($"File: {file.FullName} successfully copied");

                        repositoryConfig.AddedFiles.Add(relativePath);
                    }
                    else
                    {
                        log?.TRACE("File already exists in destination and has the same content.");
                    }
                }

                repositoryConfig.RemovedFiles.AddRange(existedFiles);

                log?.INFO($"Copying files from: {tutorial.PathToGeneratedTutorial} to: {repositoryConfig.LocalRepository} successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during copying files for tutorial: {tutorial.UniqueName}", ex);
                throw new CommandAbortException($"Error occurred during copying files for tutorial: {tutorial.UniqueName}", ex);
            }
        }

        [Command("Get Aem tags used in tutorial")]
        public List<AemTag> GetTutorialAemTags(GitHubTutorial tutorial, AemTagManager tagManager, ApiManager apiManager, AemUser user, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Getting Aem tags that used in tutorial: {tutorial.UniqueName}");
            try
            {
                var tagIds = new List<string>();
                foreach (var tutorialTest in tutorial.GitHubTutorialTests)
                {
                    var tutorialFile = tutorialTest.TutorialFile;
                    log?.TRACE($"Extracting tags for: {tutorialFile}");
                    if (tutorialFile.Tags != null)
                        foreach (var tutorialTag in tutorialFile.Tags)
                        {
                            log?.TRACE($"Extracted tag: {tutorialTag}");
                            if (!tagIds.Contains(tutorialTag))
                                tagIds.Add(tutorialTag);
                        }
                }
                log?.DEBUG($"Extracted tags count: {tagIds.Count}");

                var aemTags = new List<AemTag>();

                log?.TRACE("Get Aem tags");
                foreach (var tagId in tagIds)
                {
                    try
                    {
                        log?.TRACE($"Get Aem tag with id: {tagId}");
                        var aemTag = tagManager.GetTag(apiManager, user, landscapeConfig, tagId, log);
                        aemTags.Add(aemTag);
                        log?.TRACE($"Got Aem tag with title: {aemTag.Title}");
                    }
                    catch (Exception ex)
                    {
                        log?.WARN($"Couldn't get aem tag from id: {tagId}", ex);
                    }
                }

                log?.INFO($"Got Aem tags count: {aemTags.Count}");

                log?.INFO($"Getting Aem tags that used in tutorial: {tutorial.UniqueName} successfully completed");

                return aemTags;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during getting tags for tutorial: {tutorial.UniqueName}", ex);
                throw new CommandAbortException($"Error occurred during getting tags for tutorial: {tutorial.UniqueName}", ex);
            }
        }

        private static Regex _bodyRepoUrlRegex = new Regex(@"(?<=[\[\(])(.*?github\.com.*?)(?=[\]\)])", RegexOptions.Compiled);

        [Command("Associate GitHub Issues with GitHub tutorial test")]
        public void AssociateIssues(GitHubTutorial tutorial, List<GitHubIssue> issues, ILogger log)
        {
            log?.INFO($"Associate GitHub issues with GitHub tutorial test in tutorial: {tutorial.UniqueName}");
            try
            {
                tutorial.GitHubTutorialTests.ForEach(t => t.ActualIssues.Clear());
                foreach (var issue in issues)
                {
                    log?.DEBUG($"Parsing issue: '{issue.Title}'");
                    log?.TRACE($"Content: {issue.Content}");

                    Match match = _bodyRepoUrlRegex.Match(issue.Content);
                    if (match.Success)
                    {
                        string url = match.Groups[0].Value;

                        log?.TRACE($"Parsed url: {url}");

                        var name = url.Substring(url.LastIndexOf("/"));
                        name = name.Substring(1, name.LastIndexOf(".") - 1).ToLower();

                        log?.DEBUG($"Name of GitHub tutorial file with issue: {name}");

                        var matchedTests = tutorial.GitHubTutorialTests.Where(t => t.TutorialFile.Name.ToLower() == name).ToList();
                        log?.DEBUG($"Matched GitHub tutorial files count: {matchedTests.Count}");

                        foreach (var test in matchedTests)
                        {
                            test.ActualIssues.Add(new GitHubTutorialIssue { Title = issue.Title, Content = issue.Content });
                            log?.TRACE($"Added issue to GitHub tutorial test: {test.Name}. Total file issues: {test.ActualIssues.Count}");
                        }
                    }
                    else
                    {
                        log?.WARN($"Couldn't extract tutorial url for issue: '{issue.Title}'. Issue content:\n{issue.Content}");
                    }
                }

                log?.INFO($"Associating GitHub issues with GitHub tutorial files in tutorial: {tutorial.UniqueName} successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during associating GitHub issues with GitHub tutorial files in tutorial: {tutorial.UniqueName}", ex);
                throw new CommandAbortException($"Error occurred during associating GitHub issues with GitHub tutorial files in tutorial: {tutorial.UniqueName}", ex);
            }
        }

        [Command("Associate Tutorial cards with GitHub tutorial tests on Publish")]
        public void AssociateCardsOnPublish(GitHubTutorial tutorial, List<TutorialCard> cards, ILogger log)
        {
            log?.INFO($"Associate Tutorial cards with GitHub tutorial files on Publish in tutorial: {tutorial.UniqueName}");
            try
            {
                AssociateCards(tutorial, cards, false, log);

                log?.INFO($"Associating Tutorial cards with GitHub tutorial files on Publish in tutorial: {tutorial.UniqueName} successfully completed");
            }
            catch (Exception ex)
            {
                log?.INFO($"Error occurred during associating Tutorial cards with GitHub tutorial files on Publish in tutorial: {tutorial.UniqueName}", ex);
                throw new CommandAbortException($"Error occurred during associating Tutorial cards with GitHub tutorial files on Publish in tutorial: {tutorial.UniqueName}", ex);
            }
        }

        [Command("Associate Tutorial cards with GitHub tutorial tests on Author")]
        public void AssociateCardsOnAuthor(GitHubTutorial tutorial, List<TutorialCard> cards, ILogger log)
        {
            log?.INFO($"Associate Tutorial cards with GitHub tutorial files on Author in tutorial: {tutorial.UniqueName}");
            try
            {
                AssociateCards(tutorial, cards, false, log);

                log?.INFO($"Associating Tutorial cards with GitHub tutorial files on Author in tutorial: {tutorial.UniqueName} successfully completed");
            }
            catch (Exception ex)
            {
                log?.INFO($"Error occurred during associating Tutorial cards with GitHub tutorial files on Author in tutorial: {tutorial.UniqueName}", ex);
                throw new CommandAbortException($"Error occurred during associating Tutorial cards with GitHub tutorial files on Author in tutorial: {tutorial.UniqueName}", ex);
            }
        }

        private static void AssociateCards(GitHubTutorial tutorial, List<TutorialCard> cards, bool IsAuthor, ILogger log)
        {
            if (IsAuthor)
                tutorial.GitHubTutorialTests.ForEach(t => t.ActualCardsOnAuthor.Clear());
            else
                tutorial.GitHubTutorialTests.ForEach(t => t.ActualCardsOnPublish.Clear());

            foreach (var card in cards)
            {
                log?.DEBUG($"Associating card: '{card.Name}'");

                var matchedTests = tutorial.GitHubTutorialTests.Where(t => t.TutorialFile.Name.ToLower() == card.Name).ToList();
                log?.DEBUG($"Matched GitHub tutorial files count: {matchedTests.Count}");

                foreach (var test in matchedTests)
                {
                    var tutorialCard = MetaType.CopyObjectWithCast(card);
                    tutorialCard.Name = card.Name;
                    tutorialCard.Location = card.Location;
                    tutorialCard.Status = card.Status;
                    tutorialCard.URL = card.URL;

                    if (IsAuthor)
                    {
                        test.ActualCardsOnAuthor.Add(tutorialCard);
                        log?.TRACE($"Added card to GitHub tutorial file: {test.Name}. Total file cards: {test.ActualCardsOnAuthor.Count}");
                    }
                    else
                    {
                        test.ActualCardsOnPublish.Add(tutorialCard);
                        log?.TRACE($"Added card to GitHub tutorial file: {test.Name}. Total file cards: {test.ActualCardsOnPublish.Count}");
                    }
                }
            }
        }

        [Command("Verify tutorial issues")]
        public void VerifyTutorialIssues(GitHubTutorialTest gitHubTutorialTest, ILogger log)
        {
            log?.INFO($"Verify issues for tutorial test: {gitHubTutorialTest.Name}");
            try
            {
                if (gitHubTutorialTest.ExpectedIssue == null)
                {
                    if (gitHubTutorialTest.ActualIssues.Count == 0)
                    {
                        log?.INFO($"Tutorial test: {gitHubTutorialTest.Name} haven't any issues");
                    }
                    else
                    {
                        log?.ERROR($"Tutorial test didn't expect any issues but have: {gitHubTutorialTest.ActualIssues.Count} issues");
                        throw new CommandAbortException($"Tutorial test didn't expect any issues but have: {gitHubTutorialTest.ActualIssues.Count} issues");
                    }
                }
                else
                {
                    if (gitHubTutorialTest.ActualIssues.Count == 0)
                    {
                        log?.ERROR($"Tutorial test expect: {gitHubTutorialTest.ExpectedIssue.Title} issue but haven't any issues");
                        throw new CommandAbortException($"Tutorial test expect: {gitHubTutorialTest.ExpectedIssue.Title} issue but haven't any issues");
                    }
                    else
                    {
                        if (gitHubTutorialTest.ActualIssues.Count > 1)
                        {
                            log?.ERROR($"Tutorial test expect single: {gitHubTutorialTest.ExpectedIssue.Title} issue but have{gitHubTutorialTest.ActualIssues.Count} issues");
                            throw new CommandAbortException($"Tutorial test expect: {gitHubTutorialTest.ExpectedIssue.Title} issue but haven't any issues");
                        }
                        else
                        {
                            if (gitHubTutorialTest.ActualIssues.Count == 1)
                            {
                                if (gitHubTutorialTest.ExpectedIssue.Title == gitHubTutorialTest.ActualIssues[0].Title && gitHubTutorialTest.ExpectedIssue.Content == gitHubTutorialTest.ActualIssues[0].Content)
                                {
                                    log?.INFO($"Verify issues for tutorial test: {gitHubTutorialTest.Name}");
                                }
                                else
                                {
                                    if (gitHubTutorialTest.ExpectedIssue.Title != gitHubTutorialTest.ActualIssues[0].Title)
                                    {
                                        log?.ERROR($"Expected title: {gitHubTutorialTest.ExpectedIssue.Title} but actual{gitHubTutorialTest.ActualIssues[0].Title}");
                                        throw new CommandAbortException($"Expected title : {gitHubTutorialTest.ExpectedIssue.Title} but actual{gitHubTutorialTest.ActualIssues[0].Title}");
                                    }
                                    else
                                    {
                                        log?.ERROR($"Expected content: {gitHubTutorialTest.ExpectedIssue.Content} but actual{gitHubTutorialTest.ActualIssues[0].Content}");
                                        throw new CommandAbortException($"Expected title : {gitHubTutorialTest.ExpectedIssue.Content} but actual{gitHubTutorialTest.ActualIssues[0].Content}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during verification GitHub issues for tutorial test: {gitHubTutorialTest.Name}", ex);
                throw new CommandAbortException($"Error occurred during verifying GitHub issues for tutorial test: {gitHubTutorialTest.Name}", ex);
            }
        }

        private void VerifyTutorialCard(GitHubTutorialTest gitHubTutorialTest, List<TutorialCard> actualCards, ILogger log)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (actualCards.Count != 1)
                {
                    log?.ERROR($"Actual tutorial cards count more than one. Count {actualCards.Count}");
                    throw new CommandAbortException($"Actual tutorial cards count more than one. Count {actualCards.Count}");
                }

                log?.INFO($"Verify tutorial card: {actualCards[0].Name} with {gitHubTutorialTest.ExpectedCard.Name}");

                if (actualCards[0].Name == gitHubTutorialTest.ExpectedCard.Name)
                {
                    log?.TRACE($"Tutorial card name equals to expected name");
                }
                else
                {
                    sb.AppendLine($"Error occurred during verification tutorial card name. Actual: {actualCards[0].Name}, expected {gitHubTutorialTest.ExpectedCard.Name}");
                }

                if (actualCards[0].Description == gitHubTutorialTest.ExpectedCard.Description)
                {
                    log?.TRACE($"Tutorial card description equals to expected description");
                }
                else
                {
                    sb.AppendLine($"Error occurred during verification tutorial card description. Actual: {actualCards[0].Description}, expected {gitHubTutorialTest.ExpectedCard.Description}");
                }

                if (actualCards[0].Title == gitHubTutorialTest.ExpectedCard.Title)
                {
                    log?.TRACE($"Tutorial card title equals to expected title");
                }
                else
                {
                    sb.AppendLine($"Error occurred during verification tutorial card title. Actual: {actualCards[0].Title}, expected {gitHubTutorialTest.ExpectedCard.Title}");
                }

                if (actualCards[0].Location == gitHubTutorialTest.ExpectedCard.Location)
                {
                    log?.TRACE($"Tutorial card location equals to expected location");
                }
                else
                {
                    sb.AppendLine($"Error occurred during verification tutorial card location. Actual: {actualCards[0].Location.ToString()}, expected {gitHubTutorialTest.ExpectedCard.Location.ToString()}");
                }

                if (actualCards[0].Status == gitHubTutorialTest.ExpectedCard.Status)
                {
                    log?.TRACE($"Tutorial card status equals to expected status");
                }
                else
                {
                    sb.AppendLine($"Error occurred during verification tutorial card status. Actual: {actualCards[0].Status}, expected {gitHubTutorialTest.ExpectedCard.Status}");
                }

                if (actualCards[0].Tags.Count == gitHubTutorialTest.ExpectedCard.Tags.Count)
                {
                    for (int i = 0; i < actualCards[0].Tags.Count; i++)
                    {
                        log?.TRACE($"Verify actual tag: {actualCards[0].Tags[i]} with expected: {gitHubTutorialTest.ExpectedCard.Tags[i]}");
                        if (actualCards[0].Tags[i] == gitHubTutorialTest.ExpectedCard.Tags[i])
                        {
                            log?.TRACE($"Tutorial card tag equals to expected tag");
                        }
                        else
                        {
                            sb.AppendLine($"Error occurred during verification tutorial card tag. Actual: {actualCards[0].Tags[i]}, expected {gitHubTutorialTest.ExpectedCard.Tags[i]}");
                        }
                    }
                }
                else
                {
                    sb.AppendLine($"Actual tutorial card tags count: {actualCards[0].Tags.Count} isn't equals to expected {gitHubTutorialTest.ExpectedCard.Tags.Count} count");
                }

                if (sb.Length > 0)
                {
                    log?.ERROR($"Errors occurred during verification tutorial card:\n{sb.ToString()}");
                    throw new CommandAbortException($"Errors occurred during verification tutorial card:\n{sb.ToString()}");
                }
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during verification tutorial card: {actualCards[0].Name}", ex);
                throw new CommandAbortException($"Error occurred during verification tutorial card: {actualCards[0].Name}", ex);
            }
        }

        public void VerifyTutorialCardsOnAuthor(GitHubTutorialTest gitHubTutorialTest, ILogger log)
        {
            log?.INFO($"Verify tutorial cards on author");
            try
            {
                VerifyTutorialCard(gitHubTutorialTest, gitHubTutorialTest.ActualCardsOnAuthor, log);
                log?.DEBUG($"Verification tutorial cards on author successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during verification tutorial card on author", ex);
                throw new CommandAbortException("Error occurred during verification tutorial card on author", ex);
            }
        }

        public void VerifyTutorialCardsOnPublish(GitHubTutorialTest gitHubTutorialTest, ILogger log)
        {
            log?.INFO($"Verify tutorial cards on publish");
            try
            {
                VerifyTutorialCard(gitHubTutorialTest, gitHubTutorialTest.ActualCardsOnPublish, log);
                log?.DEBUG($"Verification tutorial cards on publish successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during verification tutorial card on publish", ex);
                throw new CommandAbortException("Error occurred during verification tutorial card on publish", ex);
            }
        }
    }
}
