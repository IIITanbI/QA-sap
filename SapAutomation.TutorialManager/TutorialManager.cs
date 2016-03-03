namespace SapAutomation.TutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;

    [CommandManager("Manager for tutorial")]
    public class TutorialManager : ICommandManager
    {
        private class LocalContainer
        {
            public string tempDir { get; set; }
        }

        ThreadLocal<LocalContainer> _container;

        public TutorialManager(TutorialManagerConfig config)
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
        public string GenerateTutorialPage(Tutorial tutorial, ILogger log)
        {
            try
            {
                log?.DEBUG($"Create tutorial page");
                var tutorialPath = Path.Combine(_container.Value.tempDir, tutorial.Folder);

                if (!Directory.Exists(tutorialPath))
                    Directory.CreateDirectory(tutorialPath);

                foreach (var tutorialItem in tutorial.TutorialItems)
                {
                    var tutorialItemPath = Path.Combine(tutorialPath, tutorialItem.FolderName);

                    if (!Directory.Exists(tutorialItemPath))
                        Directory.CreateDirectory(tutorialItemPath);

                    foreach (var tutorialFile in tutorialItem.TutorialFiles)
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
                    }
                }
                log?.DEBUG($"Creating tutorial page completed. Path: {tutorialPath}");
                return tutorialPath;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during creating tutorial page");
                throw new CommandAbortException($"Error occurred during creating tutorial page", ex);
            }
        }
    }
}
