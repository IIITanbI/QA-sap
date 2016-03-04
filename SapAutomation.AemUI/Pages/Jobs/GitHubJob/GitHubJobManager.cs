namespace SapAutomation.AemUI.Pages.Jobs.GitHubJob
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [CommandManager(typeof(GitHubJobManagerConfig), "GitHub job manager")]
    public class GitHubJobManager : ICommandManager
    {
        public WebElement GitHubComponent;

        public GitHubJobManager(GitHubJobManagerConfig config)
        {
            GitHubComponent = config.GitHubJobPageDefinition;
        }

        [Command("Command for force run github job", "ForceRunJob")]
        public void ForceRunJob(WebDriverManager wdm, ILogger log)
        {
            try
            {
                log?.DEBUG($"Force run github job");

                wdm.Click(GitHubComponent["Root.GitHubJob.ForceRunJob"], log);

                var msgBox = wdm.Find(GitHubComponent["Root.GitHubJob.StatusMsgBox"], log);
                wdm.WaitUntilElementIsVisible(msgBox, log);

                var recentResult = wdm.Find(GitHubComponent["Root.GitHubJob.RecentResult"], log);
                Equals(recentResult.Text.Contains("SUCCESS"), true);

                log?.DEBUG($"Force running github job completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during running github job");
                throw new CommandAbortException($"Error occurred during running github job", ex);
            }
        }
    }
}
