namespace SapAutomation.Managers.GitHubTutorialManager
{
    using AutoMapper;
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
        private static IMapper _mapper;

        private class LocalContainer
        {
            public string tempDir { get; set; }
        }

        ThreadLocal<LocalContainer> _container;

        static GitHubTutorialManager()
        {
            var config = new MapperConfiguration(
                cfg => cfg.CreateMap<GitHubIssue, GitHubTutorialIssue>()
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content, opts => opts.MapFrom(src => src.Content))
            );
            _mapper = config.CreateMapper();
        }

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

        private static D Map<D>(object source)
        {
            var res = _mapper.Map<D>(source);
            return res;
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
                        var listTags = new StringBuilder();

                        for (int i = 0; i < tutorialFile.Tags.Count; i++)
                        {
                            if (i != tutorialFile.Tags.Count - 1)
                                listTags.Append(tutorialFile.Tags[i] + " ");
                            else
                                listTags.Append(tutorialFile.Tags[i] + ";");
                        }

                        string[] lines =
                        {
                            "---",
                            $"title: {tutorialFile.Title}",
                            $"description: {tutorialFile.Description}",
                            $"tags: {listTags}",
                            "---",
                            $"{tutorialFile.Content}"
                        };

                        string file = Path.Combine(tutorialItemPath, tutorialFile.Name + ".md");
                        File.WriteAllLines(file, lines, Encoding.UTF8);

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

        [Command("Map Issues to Files")]
        public void MapIssueToFile(GitHubTutorial gitHubTutorial, GitManager gitManager, GitRepositoryConfig repositoryConfig, ILogger log)
        {
            var issues = gitManager.GetIssues(repositoryConfig, log);

            var map = new Dictionary<string, GitHubTutorialFile>();
            foreach (var tutitem in gitHubTutorial.GitHubTutorialItems)
            {
                foreach (var tutfile in tutitem.GitHubTutorialFiles)
                {
                    var path = Path.Combine(gitHubTutorial.Folder, tutitem.FolderName, tutfile.Name);
                    map[path] = tutfile;
                }
            }

            log?.DEBUG($"Start map issues. Count = {issues.Count}");
            foreach (var issue in issues)
            {
                var content = issue.Content;
                var regex = @"(?<=\[).*?(?=\])";

                log?.DEBUG($"Content: {content}");
                Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    log?.DEBUG($"Match success");

                    string url = match.Groups[0].Value;
                    Console.WriteLine(url);

                    List<int> pos = new List<int>();

                    int cur = url.IndexOf("/");
                    while (cur != -1)
                    {
                        pos.Add(cur);
                        cur = url.IndexOf("/", cur + 1);
                    }
                    if (pos.Count < 6)
                    {
                        log?.DEBUG($"Invalid path {url}");
                        log?.WARN($"Invalid path {url}");
                        continue;
                    }
                    log?.DEBUG($"Path ok");
                    //C:\temp\tutorials\tutorials\tutorial1\tutorial1tes2.md
                    string path = url.Substring(pos[pos.Count - 3]); //tutorials/tutorial1/tut1.md

                    var strings = path.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);

                    string file = strings[2]; //tut1.md
                    string fileFolder = strings[1]; //tutorial1
                    string mainFolder = strings[0]; //tutorials
                    //https://github.com/ksAutotests/KsTest/blob/master/tutorials/tutorial1/tut1.md

                    var _path = Path.Combine(mainFolder, fileFolder, file);
                    if (map.ContainsKey(_path))
                    {
                        var gitHubTutorialIssue = Map<GitHubTutorialIssue>(issue);
                        map[_path].Issues.Add(gitHubTutorialIssue);
                        log?.DEBUG($"Issue mapped to file: {_path}");
                    }
                    else
                    {
                        log?.DEBUG($"Issue was not mapped. File not found.");
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

        public void VerifyTutorial(GitHubTutorialFile gitHubTutorialFile, ILogger log)
        {

        }
    }
}
