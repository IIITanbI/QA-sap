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
                log?.INFO($"Error occurred during associating GitHub issues with GitHub tutorial files in tutorial: {tutorial.UniqueName}", ex);
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

        //[Command("Verify issues for tutorial md files")]
        //public void VerifyTutorialIssues(GitHubTutorial gitHubTutorial, List<GitHubIssue> issues, ILogger log)
        //{
        //    try
        //    {
        //        log?.INFO($"Start verification tutorial file issues for tutorial: {gitHubTutorial.UniqueName}");

        //        var issuesDict = new Dictionary<string, List<GitHubIssue>>();

        //        log?.DEBUG($"Start parsing GitHub issues");
        //        foreach (var issue in issues)
        //        {
        //            log?.TRACE($"Parsing issue: '{issue.Title}'");
        //            log?.TRACE($"Content: {issue.Content}");

        //            Match match = _bodyRepoUrlRegex.Match(issue.Content);
        //            if (match.Success)
        //            {
        //                string url = match.Groups[0].Value;

        //                log?.TRACE($"Match success: {url}");

        //                var name = url.Substring(url.LastIndexOf("/"));
        //                name = name.Substring(1, name.LastIndexOf(".") - 1).ToLower();

        //                if (!issuesDict.ContainsKey(name))
        //                    issuesDict.Add(name, new List<GitHubIssue>());
        //                issuesDict[name].Add(issue);
        //                log?.DEBUG($"Issue with key: {name} added to dictionary");
        //            }
        //            else
        //            {
        //                log?.WARN($"Couldn't extract tutorial url for issue: '{issue.Title}'. Issue content:\n{issue.Content}");
        //            }
        //        }
        //        log?.DEBUG($"Parsing GitHub issues completed");

        //        log?.DEBUG($"Start verification");

        //        var failedDict = new Dictionary<GitHubTutorialFile, List<GitHubIssue>>();
        //        foreach (var tutorialTest in gitHubTutorial.GitHubTutorialTests)
        //        {
        //            var tutorialFile = tutorialTest.TutorialFile;
        //            log?.DEBUG($"Verify tutorial: {tutorialFile.Key}");
        //            log?.DEBUG($"Should tutorial have issue? : {tutorialFile.Value.HaveIssue}");

        //            var machedIssues = issuesDict.ContainsKey(tutorialFile.Key.ToLower())
        //                ? issuesDict[tutorialFile.Key.ToLower()]
        //                : null;

        //            log?.TRACE($"Found: {machedIssues?.Count ?? 0} matched issue");
        //            if (!tutorialFile.Value.HaveIssue)
        //            {
        //                if (machedIssues != null)
        //                {
        //                    log?.ERROR($"Tutorial '{tutorialFile.Key}' shouldn't have issue but have");
        //                    failedDict.Add(tutorialFile.Value, machedIssues);
        //                }
        //                else
        //                {
        //                    log?.DEBUG($"Tutorial '{tutorialFile.Key}' doesn't have issue as expected");
        //                }
        //            }
        //            else
        //            {
        //                if (machedIssues != null)
        //                {
        //                    log?.DEBUG($"Tutorial '{tutorialFile.Key}' has issue as expected");
        //                }
        //                else
        //                {
        //                    log?.ERROR($"Tutorial '{tutorialFile.Key}' should have issue but doesn't have");
        //                    failedDict.Add(tutorialFile.Value, machedIssues);
        //                }
        //            }
        //        }

        //        if (failedDict.Count > 0)
        //        {
        //            var sb = new StringBuilder();
        //            sb.AppendLine("Verification failed. Failed items:");
        //            foreach (var failedItem in failedDict)
        //            {
        //                sb.AppendLine($"Tutorial file name: '{failedItem.Key.Name}', Title: '{failedItem.Key.Title}', Should have issue? : {failedItem.Key.HaveIssue}");
        //                sb.AppendLine($"Issues count: {failedItem.Value?.Count ?? 0}. Issues:");
        //                if (failedItem.Value != null)
        //                {
        //                    foreach (var issue in failedItem.Value)
        //                    {
        //                        sb.AppendLine($"Issue title: '{issue.Title}'");
        //                        sb.AppendLine($"Issue content: '{issue.Content}'");
        //                    }
        //                }
        //            }

        //            log?.ERROR(sb.ToString());
        //            throw new CommandAbortException(sb.ToString());
        //        }

        //        log?.INFO($"Verification for tutorial file issues successfully completed");
        //    }
        //    catch (CommandAbortException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        log?.ERROR($"Error occurred during verification tutorial file issues for tutorial: {gitHubTutorial.UniqueName}", ex);
        //        throw new CommandAbortException($"Error occurred during verification tutorial file issues for tutorial: {gitHubTutorial.UniqueName}", ex);
        //    }
        //}

        //[Command("Map TutorialCard to GitHubTutorialFile")]
        //public void VerifyTutorialCards(GitHubTutorial tutorial, List<TutorialCard> tutorialCards, List<AemTag> aemTags, ILogger log)
        //{
        //    log?.INFO("Start verification TutorialCards for GitHubTutorialFile");

        //    log?.DEBUG("Start parsing tutorial card names");
        //    var cardsDict = new Dictionary<string, List<TutorialCard>>();
        //    foreach (var card in tutorialCards)
        //    {
        //        if (!cardsDict.ContainsKey(card.Name))
        //            cardsDict.Add(card.Name, new List<TutorialCard>());
        //        cardsDict[card.Name].Add(card);
        //        log?.DEBUG($"Card with name: {card.Name} added to dictionary");
        //    }
        //    log?.DEBUG("Tutorial card names parsed");

        //    log?.DEBUG("Start verification");

        //    var failedDict = new Dictionary<GitHubTutorialFile, List<TutorialCard>>();
        //    var failReasons = new Dictionary<GitHubTutorialFile, string>();
        //    foreach (var tutorialFile in tutorial.TutorialFiles)
        //    {
        //        log?.DEBUG($"Search cards for file: {tutorialFile.Key}");

        //        var machedCards = cardsDict.ContainsKey(tutorialFile.Key.ToLower())
        //            ? cardsDict[tutorialFile.Key.ToLower()]
        //            : null;

        //        log?.DEBUG($"Found cards count: {machedCards?.Count ?? 0}");

        //        if (tutorialFile.Value.HaveCard)
        //        {
        //            if (machedCards == null)
        //            {
        //                log?.ERROR($"Tutorial '{tutorialFile.Key}' should have card but doesn't have");
        //                failedDict.Add(tutorialFile.Value, machedCards);
        //                failReasons.Add(tutorialFile.Value, $"Tutorial '{tutorialFile.Key}' should have card but doesn't have");
        //            }
        //            else
        //            {
        //                log?.DEBUG($"Tutorial '{tutorialFile.Key}' has tutorial card as expected");
        //                if (machedCards.Count > 1)
        //                {
        //                    log?.ERROR($"Matched cards count more that 1. Actual count: {machedCards.Count}");
        //                    failReasons.Add(tutorialFile.Value, $"Matched cards count more that 1. Actual count: {machedCards.Count}");
        //                    failedDict.Add(tutorialFile.Value, machedCards);
        //                }
        //                else
        //                {
        //                    log?.DEBUG("Start content verification");
        //                    var card = machedCards[0];
        //                    var reason = new StringBuilder();
        //                    if (tutorialFile.Value.Title.Trim() != card.Title.Trim())
        //                    {
        //                        reason.AppendLine($"Tutorial file '{tutorialFile.Key}' has following title: '{tutorialFile.Value.Title}'\nbut card has following title: '{card.Title}'");
        //                    }
        //                    if (tutorialFile.Value.Description.Trim() != card.Description.Trim())
        //                    {
        //                        reason.AppendLine($"Tutorial file '{tutorialFile.Key}' has following description:\n'{tutorialFile.Value.Title}'\nbut card has following title:\n'{card.Title}'");
        //                    }
        //                    if (reason.Length != 0)
        //                    {
        //                        log?.ERROR($"Content of card for tutorial file '{tutorialFile.Key}' doesn't match expected content\n{reason.ToString()}");
        //                        failReasons.Add(tutorialFile.Value, $"Content of card for tutorial file '{tutorialFile.Key}' doesn't match expected content\n{reason.ToString()}");
        //                        failedDict.Add(tutorialFile.Value, machedCards);
        //                    }
        //                    else
        //                    {
        //                        log?.INFO($"Content of card for tutorial file '{tutorialFile.Key}' is equal to expected content");
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (machedCards == null)
        //            {
        //                log?.INFO($"Tutorial file '{tutorialFile.Key}' doesn't have card as expected");
        //            }
        //            else
        //            {
        //                log?.ERROR($"Tutorial file '{tutorialFile.Key}' has card but shouldn't");
        //                failReasons.Add(tutorialFile.Value, $"Tutorial file '{tutorialFile.Key}' has card but shouldn't");
        //                failedDict.Add(tutorialFile.Value, machedCards);
        //            }
        //        }
        //    }

        //    if (failedDict.Count > 0)
        //    {
        //        var sb = new StringBuilder();
        //        sb.AppendLine("Tutorial Cards verification was completed with errors for some tutorial files");

        //        foreach (var failedItem in failedDict)
        //        {
        //            sb.AppendLine($"Tutorial file name: {failedItem.Key.Name}");
        //            sb.AppendLine($"Fail reason: {failReasons[failedItem.Key]}");
        //            sb.AppendLine($"Tutorial file title: {failedItem.Key.Title}");
        //            sb.AppendLine($"Tutorial file description: {failedItem.Key.Description}");
        //            sb.AppendLine($"Tutorial file content: {failedItem.Key.Content}");
        //            sb.AppendLine($"Matched cards count: {failedItem.Value?.Count ?? 0}");
        //            if (failedItem.Value != null)
        //            {
        //                sb.AppendLine($"Matched cards info:");
        //                var counter = 1;
        //                foreach (var card in failedItem.Value)
        //                {
        //                    sb.AppendLine($"Card #{counter++}");
        //                    sb.AppendLine($"Card name: {card.Name}");
        //                    sb.AppendLine($"Card title: {card.Title}");
        //                    sb.AppendLine($"Card URL: {card.URL}");
        //                    sb.AppendLine($"Card description: {card.Description}");
        //                    sb.AppendLine($"Card status: {card.Status}");
        //                }
        //            }
        //        }

        //        log?.ERROR(sb.ToString());
        //        throw new CommandAbortException(sb.ToString());
        //    }
        //    else
        //    {
        //        log?.INFO("Tutorial Cards verification was successfully completed");
        //    }
        //}
    }
}
