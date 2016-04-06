namespace SapAutomation.Managers.JobManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.Managers.ApiManager;
    using QA.AutomatedMagic.CommandsMagic;
    using AemUserManager;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System;
    using System.Diagnostics;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Threading;
    [CommandManager("Aem job manager")]
    public class JobManager : BaseCommandManager
    {
        private static object _lock = new object();

        [Command("Command for run github job")]
        public void RunGitHubJob(ApiManager apiManager, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            log?.INFO("Run GitHubJob");
            try
            {
                var req = new Request()
                {
                    ContentType = "text/html;charset=UTF-8",
                    Method = Request.Methods.GET,
                    PostData = "/bin/sapdx/github/admin?action=force-job"
                };

                Response resp = null;
                lock (_lock)
                {
                    resp = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.Username, user.Password, log);
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
                    throw new CommandAbortException($"Failed to parse job result. Content:\n{resp.Content}", ex);
                }

                log?.INFO("GitHubJob successfully completed");
            }
            catch (Exception ex)
            {
                log.ERROR("Error occurred during running GitHubJob", ex);
                throw new CommandAbortException("Error occurred during running GitHubJob", ex);
            }
        }

        [Command("WaitForGitHubJob")]
        public void WaitForGitHubJob(ApiManager apiManager, LandscapeConfig landscapeConfig, AemUser user, string timeoutInSec, ILogger log)
        {
            log?.INFO($"Waiting for GitHub job complete execution with timeout: {timeoutInSec} seconds");
            try
            {
                var timeout = int.Parse(timeoutInSec);


                var req = new Request()
                {
                    ContentType = "text/html;charset=UTF-8",
                    Method = Request.Methods.GET,
                    PostData = "/etc/sapdx/tools/gitHubAdmin.3.json"
                };

                Response resp = null;
                var sw = Stopwatch.StartNew();
                lock (_lock)
                {
                    while (true)
                    {
                        resp = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, req, user.Username, user.Password, log);
                        var re = JObject.Parse(resp.Content);
                        var status = re["jcr:content"];
                        if (status != null)
                        {
                            var progress = status["progress"];
                            if (progress == null)
                            {
                                break;
                            }
                            else
                            {
                                log?.DEBUG($"Job progress message: {progress.ToString()}");

                                if (sw.Elapsed.Seconds < timeout)
                                {
                                    log?.DEBUG($"Sleep for 10 seconds");
                                    Thread.Sleep(10000);
                                }
                                else
                                {
                                    log?.ERROR($"Timeout {timeoutInSec} seconds reached");
                                    throw new CommandAbortException($"Timeout {timeoutInSec} seconds reached");
                                }
                            }
                        }
                        else
                        {
                            log?.ERROR($"Response parse error. Couldn't find element 'jcr:content' in response content\n{re.ToString()}");
                            throw new CommandAbortException($"Response parse error. Couldn't find element 'jcr:content' in response content\n{re.ToString()}");
                        }
                    }
                }

                log?.INFO($"Waiting for GitHub job complete execution with timeout: {timeoutInSec} seconds completed successfully");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during waiting for GitHub job complete execution", ex);
                throw new CommandAbortException($"Error occurred during waiting for GitHub job complete execution", ex);
            }
        }
    }
}
