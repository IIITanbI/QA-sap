namespace SapAutomation.Managers.JobManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.ApiManager;
    using QA.AutomatedMagic.CommandsMagic;
    using AemUserManager;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System;
    [CommandManager("Aem job manager")]
    public class JobManager : BaseCommandManager
    {
        private static object _lock = new object();

        [Command("Command for run github job", "Run github job")]
        public void RunGitHubJob(ApiManager aManager, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            try
            {
                log?.INFO("Run GitHubJob");
                var req = new Request()
                {
                    ContentType = "text/html;charset=UTF-8",
                    Method = Request.Methods.GET,
                    PostData = "/bin/sapdx/github/admin?action=force-job"
                };

                Response resp = null;
                lock (_lock)
                {
                    resp = aManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.Username, user.Password, log);
                }

                try
                {
                    var responceXml = XElement.Parse(resp.Content);
                    var message = responceXml.XPathSelectElement("//div[@id='Message']")?.Value ?? "FAILED";
                    var status = responceXml.XPathSelectElement("//div[@id='Status']")?.Value ?? "666";

                    if (message != "success" || status != "200")
                        throw new CommandAbortException($"Job execution failed. Status: {status}. Message: {message}");
                }
                catch (CommandAbortException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw new CommandAbortException($"Failed to parse job result. Content:\n{resp.Content}");
                }

                log?.INFO("GitHubJob successfully completed");
            }
            catch (Exception ex)
            {
                log.ERROR("Error occurred during running GitHubJob", ex);
                throw new CommandAbortException("Error occurred during running GitHubJob", ex);
            }
        }
    }
}
