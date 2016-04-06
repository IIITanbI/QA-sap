namespace SapAutomation.Managers.GitHubTutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.Managers.GitManager;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic.Managers.WebDriverManager;
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

                if (tutorial.TutorialAction == GitHubTutorialAction.Create)
                    repositoryConfig.RemovedFiles.AddRange(existedFiles);

                log?.INFO($"Copying files from: {tutorial.PathToGeneratedTutorial} to: {repositoryConfig.LocalRepository} successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during copying files for tutorial: {tutorial.UniqueName}", ex);
                throw new CommandAbortException($"Error occurred during copying files for tutorial: {tutorial.UniqueName}", ex);
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
                AssociateCards(tutorial, cards, true, log);

                log?.INFO($"Associating Tutorial cards with GitHub tutorial files on Author in tutorial: {tutorial.UniqueName} successfully completed");
            }
            catch (Exception ex)
            {
                log?.INFO($"Error occurred during associating Tutorial cards with GitHub tutorial files on Author in tutorial: {tutorial.UniqueName}", ex);
                throw new CommandAbortException($"Error occurred during associating Tutorial cards with GitHub tutorial files on Author in tutorial: {tutorial.UniqueName}", ex);
            }
        }

        private void AssociateCards(GitHubTutorial tutorial, List<TutorialCard> cards, bool IsAuthor, ILogger log)
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

        [Command("Verify tutorial issue")]
        public void VerifyIssue(GitHubTutorialTest gitHubTutorialTest, ILogger log)
        {
            log?.INFO($"Verify issues for tutorial test: '{gitHubTutorialTest.Name}'");
            log?.INFO($"Tutorial file name: '{gitHubTutorialTest.TutorialFile.Name}'");
            if (gitHubTutorialTest.ExpectedIssue == null)
            {
                if (gitHubTutorialTest.ActualIssues.Count == 0)
                {
                    log?.INFO($"Tutorial test: {gitHubTutorialTest.Name} haven't any issues as expected");
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.AppendLine($"Tutorial test mustn't have issue, but have {gitHubTutorialTest.ActualIssues.Count} issues");
                    sb.AppendLine($"Unexpected issues:");
                    foreach (var issue in gitHubTutorialTest.ActualIssues)
                    {
                        sb.AppendLine($"Issue with title: '{issue.Title}'");
                        sb.AppendLine($"Issue content: {issue.Content}");
                    }

                    log?.ERROR(sb.ToString());
                    throw new CommandAbortException(sb.ToString());
                }
            }
            else
            {
                if (gitHubTutorialTest.ActualIssues.Count == 0)
                {
                    var sb = new StringBuilder($"Tutorial test expect issue but hasn't any issues");
                    sb.AppendLine($"Expected issue title: {gitHubTutorialTest.ExpectedIssue.Title}");
                    sb.AppendLine($"Expected issue content: {gitHubTutorialTest.ExpectedIssue.Content}");
                    log?.ERROR(sb.ToString());
                    throw new CommandAbortException(sb.ToString());
                }
                else
                {
                    var sb = new StringBuilder();
                    if (gitHubTutorialTest.ActualIssues.Count > 1)
                    {
                        sb.AppendLine($"Expected 1 issue, but actually there are {gitHubTutorialTest.ActualIssues.Count} issues"); sb.AppendLine($"Unexpected issues:");
                        sb.AppendLine($"Issues:");
                        foreach (var issue in gitHubTutorialTest.ActualIssues)
                        {
                            sb.AppendLine($"Issue with title: '{issue.Title}'");
                            sb.AppendLine($"Issue content: {issue.Content}");
                        }

                        log?.WARN(sb.ToString());
                    }

                    var actualIssue = gitHubTutorialTest.ActualIssues[0];
                    //TODO some logic for issue verification

                    log?.INFO($"Tutorial test: '{gitHubTutorialTest.Name}' contains issue as expected");
                }
            }

            log?.INFO($"Verification for issues for tutorial test: '{gitHubTutorialTest.Name}' successfully completed");
        }

        private void VerifyCard(GitHubTutorialTest gitHubTutorialTest, List<TutorialCard> actualCards, ILogger log)
        {
            log?.DEBUG($"Verify card for tutorial test: {gitHubTutorialTest.Name}");
            log?.DEBUG($"Tutorial file name: {gitHubTutorialTest.TutorialFile.Name}");

            StringBuilder sb = new StringBuilder();

            if (gitHubTutorialTest.ExpectedCard == null)
            {
                if (actualCards.Count > 0)
                {
                    sb.AppendLine($"Expected that tutorial test: '{gitHubTutorialTest.Name}' doesn't have card, but there are {actualCards.Count} cards");
                    sb.AppendLine("Unexpected cards:");
                    foreach (var card in actualCards)
                    {
                        sb.AppendLine(card.ToString());
                    }
                    log?.ERROR(sb.ToString());
                    throw new CommandAbortException(sb.ToString());
                }
                else
                {
                    log?.INFO($"Tutorial test: '{gitHubTutorialTest.Name}' doesn't have card as expected");
                }
            }
            else
            {
                if (actualCards.Count == 0)
                {
                    sb.AppendLine("There are no actual cards, but one is expected");
                    sb.AppendLine("Expected card:");
                    sb.AppendLine(gitHubTutorialTest.ExpectedCard.ToString());
                    log?.ERROR(sb.ToString());
                    throw new CommandAbortException(sb.ToString());
                }
                if (actualCards.Count > 1)
                {
                    var wsb = new StringBuilder();
                    wsb.AppendLine($"Actual tutorial cards count more than one. Count {actualCards.Count}");
                    wsb.AppendLine("Cards");
                    foreach (var card in actualCards)
                    {
                        wsb.AppendLine(card.ToString());
                    }
                    log?.WARN(wsb.ToString());
                }
                var actualCard = actualCards[0];

                if ((actualCard.Description ?? "Not Specified") != (gitHubTutorialTest.ExpectedCard.Description ?? "Not Specified"))
                {
                    sb.AppendLine($"Expected card description is not equal to actual");
                    sb.AppendLine($"Expected card description: '{gitHubTutorialTest.ExpectedCard.Description}'");
                    sb.AppendLine($"Actual card description: '{actualCard.Description}'");
                }

                if ((actualCard.Title ?? "Not Specified") != (gitHubTutorialTest.ExpectedCard.Title ?? "Not Specified"))
                {
                    sb.AppendLine($"Expected card title is not equal to actual");
                    sb.AppendLine($"Expected card title: '{gitHubTutorialTest.ExpectedCard.Title}'");
                    sb.AppendLine($"Actual card title: '{actualCard.Title}'");
                }

                var expectedTags = gitHubTutorialTest.ExpectedCard.Tags ?? new List<string>();
                var actualTags = actualCard.Tags ?? new List<string>();
                if (expectedTags.Count != actualTags.Count)
                {
                    sb.AppendLine($"Expected tags count: {expectedTags.Count} is not equal to actual tags count: {actualTags.Count}");
                }
                var extraTags = actualTags.Where(t => !expectedTags.Contains(t)).ToList();
                var missedTags = expectedTags.Where(t => !actualTags.Contains(t)).ToList();

                if (extraTags.Count > 0)
                {
                    sb.AppendLine("Extra tags (additional tags in Actual tags):");
                    foreach (var tag in extraTags)
                    {
                        sb.AppendLine($"Extra tag: '{tag}'");
                    }
                }

                if (missedTags.Count > 0)
                {
                    sb.AppendLine("Missed tags (not present tags in Actual tags):");
                    foreach (var tag in extraTags)
                    {
                        sb.AppendLine($"Missed tag: '{tag}'");
                    }
                }

                if (sb.Length > 0)
                {
                    sb.AppendLine("All Actual cards:");
                    foreach (var card in actualCards)
                    {
                        sb.AppendLine(card.ToString());
                    }
                    log?.ERROR(sb.ToString());
                    throw new CommandAbortException(sb.ToString());
                }
            }

            log?.DEBUG($"Verification for card for tutorial test: '{gitHubTutorialTest.Name}' successfully completed");
        }

        [Command("Verify card page")]
        public void VerifyCard(GitHubTutorialTest gitHubTutorialTest, TutorialCard actualCard, ILogger log)
        {
            log?.DEBUG($"Verify card for tutorial test: {gitHubTutorialTest.Name}");
            log?.DEBUG($"Tutorial file name: {gitHubTutorialTest.TutorialFile.Name}");

            StringBuilder sb = new StringBuilder();

            if (gitHubTutorialTest.ExpectedCard == null)
            {
                log?.INFO($"There is no expected card for tutorial test: {gitHubTutorialTest.Name} so PASSED");
                return;
            }
            else
            {
                if ((actualCard.Description ?? "Not Specified") != (gitHubTutorialTest.ExpectedCard.Description ?? "Not Specified"))
                {
                    sb.AppendLine($"Expected card description is not equal to actual");
                    sb.AppendLine($"Expected card description: '{gitHubTutorialTest.ExpectedCard.Description}'");
                    sb.AppendLine($"Actual card description: '{actualCard.Description}'");
                }

                if ((actualCard.Title ?? "Not Specified") != (gitHubTutorialTest.ExpectedCard.Title ?? "Not Specified"))
                {
                    sb.AppendLine($"Expected card title is not equal to actual");
                    sb.AppendLine($"Expected card title: '{gitHubTutorialTest.ExpectedCard.Title}'");
                    sb.AppendLine($"Actual card title: '{actualCard.Title}'");
                }

                if ((actualCard.Content ?? "Not Specified") != (gitHubTutorialTest.ExpectedCard.Content ?? "Not Specified"))
                {
                    sb.AppendLine($"Expected card content is not equal to actual");
                    sb.AppendLine($"Expected card content: '{gitHubTutorialTest.ExpectedCard.Content}'");
                    sb.AppendLine($"Actual card content: '{actualCard.Content}'");
                }

                var expectedTags = gitHubTutorialTest.ExpectedCard.Tags ?? new List<string>();
                var actualTags = actualCard.Tags ?? new List<string>();
                if (expectedTags.Count != actualTags.Count)
                {
                    sb.AppendLine($"Expected tags count: {expectedTags.Count} is not equal to actual tags count: {actualTags.Count}");
                }
                var extraTags = actualTags.Where(t => !expectedTags.Contains(t)).ToList();
                var missedTags = expectedTags.Where(t => !actualTags.Contains(t)).ToList();

                if (extraTags.Count > 0)
                {
                    sb.AppendLine("Extra tags (additional tags in Actual tags):");
                    foreach (var tag in extraTags)
                    {
                        sb.AppendLine($"Extra tag: '{tag}'");
                    }
                }

                if (missedTags.Count > 0)
                {
                    sb.AppendLine("Missed tags (not present tags in Actual tags):");
                    foreach (var tag in extraTags)
                    {
                        sb.AppendLine($"Missed tag: '{tag}'");
                    }
                }

                if (sb.Length > 0)
                {
                    log?.ERROR(sb.ToString());
                    throw new CommandAbortException(sb.ToString());
                }
            }

            log?.DEBUG($"Verification for card for tutorial test: '{gitHubTutorialTest.Name}' successfully completed");
        }

        [Command("VerifyTutorialCardsOnAuthor")]
        public void VerifyTutorialCardOnAuthor(GitHubTutorialTest gitHubTutorialTest, ILogger log)
        {
            log?.INFO($"Verify tutorial cards on author");
            try
            {
                VerifyCard(gitHubTutorialTest, gitHubTutorialTest.ActualCardsOnAuthor, log);
                log?.INFO($"Verification tutorial cards on author successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during verification tutorial card on Author", ex);
                throw new CommandAbortException("Error occurred during verification tutorial card on Author", ex);
            }
        }

        [Command("VerifyTutorialCardsOnPublish")]
        public void VerifyTutorialCardOnPublish(GitHubTutorialTest gitHubTutorialTest, ILogger log)
        {
            log?.INFO($"Verify tutorial cards on publish");
            try
            {
                VerifyCard(gitHubTutorialTest, gitHubTutorialTest.ActualCardsOnPublish, log);
                log?.INFO($"Verification tutorial cards on publish successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during verification tutorial card on publish", ex);
                throw new CommandAbortException("Error occurred during verification tutorial card on publish", ex);
            }
        }

        [Command("OpenTutorialCardOnAuthor")]
        public void OpenTutorialCardOnAuthor(WebDriverManager webDriverManager, GitHubTutorialTest gitHubTutorialTest, ILogger log)
        {
            log?.INFO($"Open tutorial card for tutorial test: '{gitHubTutorialTest.Name}' on Author");
            log?.DEBUG($"Tutorial file name: {gitHubTutorialTest.TutorialFile.Name}");

            try
            {
                OpenTutorialCard(webDriverManager, gitHubTutorialTest.ActualCardsOnAuthor, gitHubTutorialTest.ExpectedCard, log);
            }
            catch (Exception ex)
            {
                log?.INFO($"Error occurred during opening tutorial card for tutorial test: '{gitHubTutorialTest.Name}' on Author", ex);
                throw new CommandAbortException($"Error occurred during opening tutorial card for tutorial test: '{gitHubTutorialTest.Name}' on Author", ex);
            }
        }
        [Command("OpenTutorialCardOnPublish")]
        public void OpenTutorialCardOnPublish(WebDriverManager webDriverManager, GitHubTutorialTest gitHubTutorialTest, ILogger log)
        {
            log?.INFO($"Open tutorial card for tutorial test: '{gitHubTutorialTest.Name}' on Publish");
            log?.DEBUG($"Tutorial file name: {gitHubTutorialTest.TutorialFile.Name}");

            try
            {
                OpenTutorialCard(webDriverManager, gitHubTutorialTest.ActualCardsOnPublish, gitHubTutorialTest.ExpectedCard, log);
            }
            catch (Exception ex)
            {
                log?.INFO($"Error occurred during opening tutorial card for tutorial test: '{gitHubTutorialTest.Name}' on Publish", ex);
                throw new CommandAbortException($"Error occurred during opening tutorial card for tutorial test: '{gitHubTutorialTest.Name}' on Publish", ex);
            }
        }
        private void OpenTutorialCard(WebDriverManager webDriverManager, List<TutorialCard> actualCards, TutorialCard expectedCard, ILogger log)
        {
            if (expectedCard == null)
            {
                log?.INFO($"There is no expected card so PASSED");
                return;
            }
            else
            {
                if (actualCards.Count == 0)
                {
                    log?.ERROR($"There is no actual cards so FAILED");
                    throw new CommandAbortException($"There is no actual cards so FAILED");
                }
                else
                {
                    webDriverManager.Navigate(actualCards[0].URL, log);
                }
            }
        }
    }
}
