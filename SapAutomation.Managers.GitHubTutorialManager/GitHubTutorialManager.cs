namespace SapAutomation.Managers.GitHubTutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.GitManager;
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

        [Command("Create tutorial page", "GenerateTutorialPage")]
        public void GenerateTutorial(GitHubTutorial tutorial, ILogger log)
        {
            try
            {
                log?.DEBUG($"Create tutorial page");
                var tutorialPath = Path.Combine(_container.Value.tempDir, DateTime.UtcNow.ToFileTimeUtc().ToString(), tutorial.Folder);

                if (!Directory.Exists(tutorialPath))
                    Directory.CreateDirectory(tutorialPath);

                foreach (var tutorialItem in tutorial.GitHubTutorialItems)
                {
                    var tutorialItemPath = Path.Combine(tutorialPath, tutorialItem.FolderName);

                    if (!Directory.Exists(tutorialItemPath))
                        Directory.CreateDirectory(tutorialItemPath);

                    foreach (var tutorialFile in tutorialItem.GitHubTutorialFiles)
                    {
                        var sb = new StringBuilder();

                        sb.AppendLine("---");

                        if (tutorialFile.Title != null)
                            sb.AppendLine($"title: {tutorialFile.Title}");

                        if (tutorialFile.Description != null)
                            sb.AppendLine($"title: {tutorialFile.Description}");

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

                        if (tutorialFile.Content != null)
                            sb.AppendLine(tutorialFile.Content);

                        string file = Path.Combine(tutorialItemPath, tutorialFile.Name + ".md");
                        File.WriteAllText(file, sb.ToString(), Encoding.UTF8);

                        if (!tutorial.TutorialFiles.ContainsKey(tutorialFile.Name))
                            tutorial.TutorialFiles.Add(tutorialFile.Name, tutorialFile);
                        else tutorial.TutorialFiles[tutorialFile.Name] = tutorialFile;
                    }
                }

                log?.DEBUG($"Creating tutorial page completed. Path: {tutorialPath}");
                tutorial.PathToGeneratedTutorial = tutorialPath;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during creating tutorial page");
                throw new CommandAbortException($"Error occurred during creating tutorial page", ex);
            }
        }

        [Command("Copy tutorial to specified directory if files are newer", "CopyToGitRepository")]
        public void CopyToGitRepository(GitHubTutorial tutorial, GitRepositoryConfig repositoryConfig, ILogger log)
        {
            log?.INFO($"Copying files from: {tutorial.PathToGeneratedTutorial ?? "null"} to: {repositoryConfig.LocalRepository}");

            if (tutorial.PathToGeneratedTutorial == null || !Directory.Exists(tutorial.PathToGeneratedTutorial))
            {
                log?.ERROR($"Tutorial: {tutorial.UniqueName} hasn't generated yet. Possible path: {tutorial.PathToGeneratedTutorial ?? "null"}");
                throw new CommandAbortException($"Tutorial: {tutorial.UniqueName} hasn't generated yet. Possible path: {tutorial.PathToGeneratedTutorial ?? "null"}");
            }

            var di = new DirectoryInfo(tutorial.PathToGeneratedTutorial);
            var files = di.GetFiles("*.*", SearchOption.AllDirectories).ToList();
            log?.TRACE($"Found: {files.Count} files to copying");
            var targetDir = new DirectoryInfo(repositoryConfig.LocalRepository);

            var existedFileInfos = targetDir.GetFiles("*.*", SearchOption.AllDirectories);
            var existedFiles = new List<string>();
            for (int i = 0; i < existedFileInfos.Length; i++)
                existedFiles.Add(existedFileInfos[i].FullName.Replace(targetDir.FullName + "\\", ""));

            foreach (var file in files)
            {
                log?.TRACE($"Copying file: {file.FullName}");
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
                        log?.TRACE($"Files have different count of lines. So destination file will be replased");
                        needToCopy = true;
                    }
                    else
                    {
                        for (int i = 0; i < newLines.Length; i++)
                        {
                            if (newLines[i] != oldLines[i])
                            {
                                log?.TRACE($"Files are different in line № {i + 1}. So destination file will be replased");
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
                    log?.TRACE("File copied");

                    repositoryConfig.AddedFiles.Add(relativePath);
                }
                else
                {
                    log?.TRACE("File already exists in destination and has the same content.");
                }
            }

            repositoryConfig.RemovedFiles.AddRange(existedFiles);
        }


        private static Regex _bodyRepoUrlRegex = new Regex(@"(?<=[\[\(])(.*?github\.com.*?)(?=[\]\)])", RegexOptions.Compiled);

        [Command("Verify issues for tutorial md files")]
        public void VerifyTutorialIssues(GitHubTutorial gitHubTutorial, List<GitHubIssue> issues, ILogger log)
        {
            try
            {
                log?.INFO($"Start verification tutorial file issues for tutorial: {gitHubTutorial.UniqueName}");

                var issuesDict = new Dictionary<string, List<GitHubIssue>>();

                log?.DEBUG($"Start parsing GitHub issues");
                foreach (var issue in issues)
                {
                    log?.TRACE($"Parsing issue: '{issue.Title}'");
                    log?.TRACE($"Content: {issue.Content}");

                    Match match = _bodyRepoUrlRegex.Match(issue.Content);
                    if (match.Success)
                    {
                        string url = match.Groups[0].Value;

                        log?.TRACE($"Match success: {url}");

                        var name = url.Substring(url.LastIndexOf("/"));
                        name = name.Substring(name.LastIndexOf("."));

                        if (!issuesDict.ContainsKey(name))
                            issuesDict.Add(name, new List<GitHubIssue>());
                        issuesDict[name].Add(issue);
                    }
                    else
                    {
                        log?.WARN($"Couldn't extract tutorial url for issue: '{issue.Title}'. Issue content:\n{issue.Content}");
                    }
                }
                log?.DEBUG($"Parsing GitHub issues completed");

                log?.DEBUG($"Start verification");

                var failedDict = new Dictionary<GitHubTutorialFile, List<GitHubIssue>>();
                foreach (var tutorialFile in gitHubTutorial.TutorialFiles)
                {
                    log?.DEBUG($"Verify tutorial: {tutorialFile.Key}");
                    log?.DEBUG($"Should tutorial have issue? : {tutorialFile.Value.HaveIssue}");

                    var machedIssues = issuesDict.ContainsKey(tutorialFile.Key.ToLower())
                        ? issuesDict[tutorialFile.Key.ToLower()]
                        : null;

                    log?.TRACE($"Found: {machedIssues?.Count ?? 0} matched issue");
                    if (!tutorialFile.Value.HaveIssue)
                    {
                        if (machedIssues != null)
                        {
                            log?.ERROR($"Tutorial '{tutorialFile.Key}' shouldn't have issue but have");
                            failedDict.Add(tutorialFile.Value, machedIssues);
                        }
                        else
                        {
                            log?.DEBUG($"Tutorial '{tutorialFile.Key}' doesn't have issue as expected");
                        }
                    }
                    else
                    {
                        if (machedIssues != null)
                        {
                            log?.DEBUG($"Tutorial '{tutorialFile.Key}' has issue as expected");
                        }
                        else
                        {
                            log?.ERROR($"Tutorial '{tutorialFile.Key}' should have issue but doesn't have");
                            failedDict.Add(tutorialFile.Value, machedIssues);
                        }
                    }
                }

                if (failedDict.Count > 0)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("Verification failed. Failed items:");
                    foreach (var failedItem in failedDict)
                    {
                        sb.AppendLine($"Tutorial file name: '{failedItem.Key.Name}', Title: '{failedItem.Key.Title}', Should have issue? : {failedItem.Key.HaveIssue}");
                        sb.AppendLine($"Issues count: {failedItem.Value?.Count ?? 0}. Issues:");
                        if (failedItem.Value != null)
                        {
                            foreach (var issue in failedItem.Value)
                            {
                                sb.AppendLine($"Issue title: '{issue.Title}'");
                                sb.AppendLine($"Issue content: '{issue.Content}'");
                            }
                        }
                    }

                    log?.ERROR(sb.ToString());
                    throw new CommandAbortException(sb.ToString());
                }

                log?.INFO($"Verification for tutorial file issues successfully completed");
            }
            catch (CommandAbortException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during verification tutorial file issues for tutorial: {gitHubTutorial.UniqueName}", ex);
                throw new CommandAbortException($"Error occurred during verification tutorial file issues for tutorial: {gitHubTutorial.UniqueName}", ex);
            }
        }

        [Command("Map TutorialCard to GitHubTutorialFile")]
        public void VerifyTutorialCards(GitHubTutorial tutorial, List<TutorialCard> tutorialCards, ILogger log)
        {
            log?.INFO("Start verification TutorialCards to GitHubTutorialFile");

            log?.DEBUG("Start parsing tutorial card names");
            var cardsDict = new Dictionary<string, List<TutorialCard>>();
            foreach (var card in tutorialCards)
            {
                log?.TRACE($"Parsing card: '{card.Title}'");
                log?.TRACE($"URL: {card.URL}");

                var name = card.URL.Substring(card.URL.LastIndexOf("/"));
                name = name.Substring(name.LastIndexOf("."));

                log?.TRACE($"Parsed name: {name}");

                if (!cardsDict.ContainsKey(name))
                    cardsDict.Add(name, new List<TutorialCard>());
                cardsDict[name].Add(card);
            }
            log?.DEBUG("Tutorial card names parsed");

            log?.DEBUG("Start verification");

            var failedDict = new Dictionary<GitHubTutorialFile, List<TutorialCard>>();
            var failReasons = new Dictionary<GitHubTutorialFile, string>();
            foreach (var tutorialFile in tutorial.TutorialFiles)
            {
                log?.DEBUG($"Search cards for file: {tutorialFile.Key}");


                var machedCards = cardsDict.ContainsKey(tutorialFile.Key.ToLower())
                    ? cardsDict[tutorialFile.Key.ToLower()]
                    : null;

                log?.DEBUG($"Found cards count: {machedCards?.Count ?? 0}");

                if (tutorialFile.Value.HaveCard)
                {
                    if (machedCards == null)
                    {
                        log?.ERROR($"Tutorial '{tutorialFile.Key}' should have card but doesn't have");
                        failedDict.Add(tutorialFile.Value, machedCards);
                        failReasons.Add(tutorialFile.Value, $"Tutorial '{tutorialFile.Key}' should have card but doesn't have");
                    }
                    else
                    {
                        log?.DEBUG($"Tutorial '{tutorialFile.Key}' has tutorial card as expected");
                        if (machedCards.Count > 1)
                        {
                            log?.ERROR($"Matched cards count more that 1. Actual count: {machedCards.Count}");
                            failReasons.Add(tutorialFile.Value, $"Matched cards count more that 1. Actual count: {machedCards.Count}");
                        }
                        else
                        {
                            log?.DEBUG("Start content verification");
                            var card = machedCards[0];
                            var reason = new StringBuilder();
                            if (tutorialFile.Value.Title.Trim() != card.Title.Trim())
                            {
                                reason.AppendLine($"Tutorial '{tutorialFile.Key}' has following title: '{tutorialFile.Value.Title}'\nbut card has following title: '{card.Title}'");
                            }
                            if(tutorialFile.Value.Description.Trim() != card.Description.Trim())
                            {
                                reason.AppendLine($"Tutorial '{tutorialFile.Key}' has following description:\n'{tutorialFile.Value.Title}'\nbut card has following title:\n'{card.Title}'");
                            }
                        }
                    }
                }
                else
                {
                    if (machedCards == null)
                    {

                    }
                    else
                    {

                    }
                }

                //TODO verification
            }
            log?.INFO("Mapping was successfully completed");
        }
    }
}
