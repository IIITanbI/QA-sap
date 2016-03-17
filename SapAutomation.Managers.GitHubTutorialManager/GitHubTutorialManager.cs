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
        public void GenerateTutorialPage(GitHubTutorial tutorial, ILogger log)
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
                                    listTags.Append(aemTag.GetFullName());
                                }
                                else
                                {
                                    listTags.Append(", ");
                                    listTags.Append(aemTag.GetFullName());
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

        [Command("Map Issues to Files")]
        public void MapIssueToFile(GitHubTutorial gitHubTutorial, GitManager gitManager, GitRepositoryConfig repositoryConfig, ILogger log)
        {
            var issues = gitManager.GetIssues(repositoryConfig, log);

            log?.DEBUG($"Start map issues. Count = {issues.Count}");
            foreach (var issue in issues)
            {
                var content = issue.Content;
                log?.DEBUG($"Content: {content}");

                Match match = _bodyRepoUrlRegex.Match(content);
                if (match.Success)
                {
                    string url = match.Groups[0].Value;

                    log?.DEBUG($"Match success: {url}");

                    var name = url.Substring(url.LastIndexOf("/"));
                    name = name.Substring(name.LastIndexOf("."));

                    foreach (var tutorialFile in gitHubTutorial.TutorialFiles)
                    {
                        if (tutorialFile.Key.ToLower() == name.ToLower())
                            tutorialFile.Value.Issues.Add(new GitHubTutorialIssue { Title = issue.Title, Content = issue.Content });
                    }
                }
            }
        }

        [Command("Map TutorialCard to GitHubTutorialFile")]
        public void MapTutorialCardToGitHubTutorialFile(GitHubTutorial tutorial, List<TutorialCard> tutorialCards, ILogger log)
        {
            log?.INFO("Start mapping TutorialCards to GitHubTutorialFile");
            foreach (var tutorialFile in tutorial.TutorialFiles)
            {
                log?.DEBUG($"Search cards for file: {tutorialFile.Key}");
                var suitableCards = tutorialCards.Where(c => c.Name.ToLower() == tutorialFile.Key.ToLower()).ToList();
                log?.DEBUG($"Found cards count: {suitableCards.Count}");
                tutorialFile.Value.Cards = suitableCards;
            }
            log?.INFO("Mapping was successfully completed");
        }
    }
}
