namespace SapAutomation.Managers.AemPageManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.Managers.ApiManager;
    using QA.AutomatedMagic.Managers.WebDriverManager;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System;
    using AemUserManager;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    using System.Diagnostics;
    using System.Threading;
    using System.Text;

    [CommandManager(typeof(AemPageManagerConfig), "Manager for aem pages")]
    public class AemPageManager : BaseCommandManager
    {
        public AemPageManagerConfig Config { get; set; }
        public AemPageManager(AemPageManagerConfig config)
        {
            Config = config;
        }

        private void CheckAuthorization(Request request, AemUser user)
        {
            user.CheckAuthorization();
            request.Cookie = user.Cookie;
        }

        [Command("Create AEM page", "CreatePage")]
        public void CreatePage(ApiManager apiManager, AemPage aemPage, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            log?.INFO($"Create page with title:' {aemPage.Title}'");

            var cmd = $"/bin/wcmcommand?cmd=createPage&parentPath={aemPage.ParentPath}&title={aemPage.Title.ToLower()}&template={aemPage.Template}";

            log?.TRACE($"Command for page creation: {cmd}");

            var request = new Request
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = cmd
            };

            CheckAuthorization(request, user);
            var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.Username, user.Password, log);

            CheckResponseStatus(response, log);
            aemPage.Path = GetPagePath(response, log);

            log?.INFO($"Page with title:' {aemPage.Title}' successfully created");
        }

        [Command("Delete AEM page", "CreatePage")]
        public void DeletePage(ApiManager apiManager, AemPage aemPage, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            log?.INFO($"Delete page with title:' {aemPage.Title}'");

            var cmd = $"/bin/wcmcommand?cmd=deletePage&path={aemPage.ParentPath}/{aemPage.Title.ToLower()}&force=true";

            log?.TRACE($"Command for page creation: {cmd}");

            var request = new Request
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = cmd
            };

            CheckAuthorization(request, user);
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.Username, user.Password, log);

            log?.INFO($"Page with title:' {aemPage.Title}' successfully deleted");
        }

        [Command("Activate AEM page", "ActivatePage")]
        public void ActivatePage(ApiManager apiManager, AemPage aemPage, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            log?.DEBUG($"Generate command for aem page '{aemPage.Title}' activation");

            var cmd = $"/bin/replicate.json?cmd=Activate&path={aemPage.ParentPath}/{aemPage.Title.ToLower()}";

            log?.TRACE($"Command for page activation: {cmd}");

            var request = new Request
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = cmd
            };

            CheckAuthorization(request, user);
            var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.Username, user.Password, log);

            CheckResponseStatus(response, log);

            log?.DEBUG($"Generating command for aem page '{aemPage.Title}' activation completed");
        }

        [Command("Deactivate AEM page", "ActivatePage")]
        public void DeactivatePage(ApiManager apiManager, AemPage aemPage, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            log?.DEBUG($"Generate command for aem page '{aemPage.Title}' deactivation");

            var cmd = $"/bin/replicate.json?cmd=Deactivate&path={aemPage.ParentPath}/{aemPage.Title.ToLower()}";

            log?.TRACE($"Command for page deactivation: {cmd}");

            var request = new Request
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = cmd
            };

            CheckAuthorization(request, user);
            var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.Username, user.Password, log);

            CheckResponseStatus(response, log);

            log?.DEBUG($"Generating command for aem page '{aemPage.Title}' deactivation completed");
        }

        [Command("Open AEM page on author", "OpenPageOnAuthor")]
        public void OpenPageOnAuthor(WebDriverManager webDriverManager, AemPage aemPage, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Open AEM page '{aemPage.Title}' on author");
            var url = $"{landscapeConfig.AuthorHostUrl}/cf#{aemPage.ParentPath}/{aemPage.Title.ToLower()}.html";
            log?.INFO($"URL: {url}");
            webDriverManager.Navigate(url, log);
            log?.DEBUG($"Opening AEM page '{aemPage.Title}' on author completed. Current url: {webDriverManager.GetCurrentUrl()}");
        }

        [Command("Open AEM page on publish", "OpenPageOnPublish")]
        public void OpenPageOnPublish(WebDriverManager webDriverManager, AemPage aemPage, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Open AEM page '{aemPage.Title}' on publish");
            var parentPath = landscapeConfig.IsProduction ? aemPage.ProdParentPath : aemPage.ParentPath;
            var url = $"{landscapeConfig.PublishHostUrl}{parentPath}/{aemPage.Title.ToLower()}.html";
            log?.INFO($"URL: {url}");
            webDriverManager.Navigate(url, log);
            log?.DEBUG($"Opening AEM page '{aemPage.Title}' on publish completed. Current url: {webDriverManager.GetCurrentUrl()}");
        }

        [Command("Wait for page being activated")]
        public void WaitForPageActivation(ApiManager apiManager, AemPage aemPage, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            try
            {
                log?.INFO($"Start waiting for activation page: '{aemPage.Title}'");
                log?.INFO($"Interval: {Config.StatusWaitInterval} seconds");
                log?.INFO($"Timeout: {Config.StatusWaitTimeout} seconds");

                var request = new Request
                {
                    ContentType = "text/html;charset=UTF-8",
                    Method = Request.Methods.GET,
                    PostData = $"{aemPage.ParentPath}.pages.json"
                };

                CheckAuthorization(request, user);

                var sw = Stopwatch.StartNew();

                while (true)
                {
                    var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.Username, user.Password, log);
                    var json = JObject.Parse(response.Content);
                    var jsonPages = (JArray)json["pages"];

                    var jsonPage = jsonPages.FirstOrDefault(jp => jp["title"].ToString().ToLower() == aemPage.Title.ToLower());

                    if (jsonPage == null)
                        throw new CommandAbortException($"Couldn't find page with name: '{aemPage.Title}' in parent: {aemPage.ParentPath}");

                    if (jsonPage["replication"].Children().Count() > 1)
                    {
                        var status = jsonPage["replication"]["action"].ToString();
                        if (status == "ACTIVATE")
                        {
                            if (jsonPage["replication"]["numQueued"].ToString() == "0")
                            {
                                log?.INFO($"Waiting for activation page: '{aemPage.Title}' successfully completed");
                                return;
                            }
                            else
                            {
                                log?.INFO($"Current number in queue: {jsonPage["replication"]["numQueued"].ToString()}");
                            }
                        }
                    }

                    if (sw.Elapsed.Seconds < Config.StatusWaitTimeout)
                    {
                        log?.DEBUG($"Page:' {aemPage.Title}' isn't activated. Sleep for: {Config.StatusWaitInterval} seconds");
                        Thread.Sleep(Config.StatusWaitInterval * 1000);
                    }
                    else
                    {
                        log?.ERROR($"Timeout reached for waiting for activation page: '{aemPage.Title}'");
                        throw new CommandAbortException($"Timeout reached for waiting for activation page: '{aemPage.Title}'");
                    }
                }
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during waiting for activation page: '{aemPage.Title}'", ex);
                throw new CommandAbortException($"Error occurred during waiting for activation page: '{aemPage.Title}'", ex);
            }
        }

        [Command("Wait for pages being activated")]
        public void WaitForChildPagesActivation(ApiManager apiManager, AemPage parentAemPage, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            try
            {
                log?.INFO($"Start waiting for activation child pages for page: '{parentAemPage.Title}'");
                var childPages = GetChildAemPages(apiManager, parentAemPage, landscapeConfig, user, log);

                log?.DEBUG($"Child pages count: {childPages.Count}");
                foreach (var aemPage in childPages)
                {
                    WaitForPageActivation(apiManager, aemPage, landscapeConfig, user, log);
                }

                log?.INFO($"Waiting for activation child pages for page: '{parentAemPage.Title}' successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during waiting for activation child pages for page: '{parentAemPage.Title}'", ex);
                throw new CommandAbortException($"Error occurred during waiting for activation child pages for page: '{parentAemPage.Title}'", ex);
            }
        }

        public void WaitForChildPagesIndexed(ApiManager apiManager, AemPage parentAemPage, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            log?.INFO($"Start waiting for indexing child pages for page: '{parentAemPage.Title}'");
            try
            {
                var childPages = GetChildAemPages(apiManager, parentAemPage, landscapeConfig, user, log);

                var req = $"{{\"search\":[],\"searchPaths\":[\"{parentAemPage.Path}\"],\"pagePath\":\"{parentAemPage.Path}\",\"filter\":[],\"tags\":[],\"sortName\":\"creationDate\",\"sortType\":\"asc\",\"pageCount\":20,\"page\":1,\"searchText\":\"\"}}";
                var type = "&type={\"isMultiSelection\":true}";
                var additionalProcess = "&additionalProcess=true";

                var request = new Request
                {
                    ContentType = "text/html;charset=UTF-8",
                    Method = Request.Methods.GET,
                    PostData = $"/bin/sapdx/solrsearch?json={req}{type}{additionalProcess}"
                };

                var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.Username, user.Password, log);

                var tmp = JObject.Parse(response.Content);
                var pageJsons = (JArray)tmp["results"];

                if (childPages.Count == pageJsons.Count)
                {
                    log?.INFO($"Waiting for indexing child pages for page: '{parentAemPage.Title}' successfully completed");
                }
                else
                {
                    log?.ERROR($"Error occurred during waiting for indexing child pages for page: '{parentAemPage.Title}'");
                    throw new CommandAbortException($"Error occurred during waiting for indexing child pages for page: '{parentAemPage.Title}'");
                }
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during waiting for activation child pages for page: '{parentAemPage.Title}'", ex);
                throw new CommandAbortException($"Error occurred during waiting for activation child pages for page: '{parentAemPage.Title}'", ex);
            }
        }

        [Command("Get child aem pages")]
        public List<AemPage> GetChildAemPages(ApiManager apiManager, AemPage aemPage, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            List<AemPage> childs = new List<AemPage>();

            log?.INFO($"Get childs of page: '{aemPage.Title}'");

            var request = new Request
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.GET,
                PostData = $"{aemPage.ParentPath}/{aemPage.Title.ToLower()}.pages.json"
            };

            var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.Username, user.Password, log);
            var tmp = JObject.Parse(response.Content);
            var pageJsons = (JArray)tmp["pages"];

            var index = 0;
            foreach (var child in pageJsons)
            {
                if (index++ == 0)
                {
                    continue;
                }

                var page = new AemPage();
                page.Title = child["title"].ToString();
                page.Path = child["path"].ToString();
                page.Template = child["templatePath"].ToString();

                page.ParentPath = $"{aemPage.ParentPath}/{aemPage.Title.ToLower()}";

                if (child["replication"].Children().Count() > 1)
                    page.Status = child["replication"]["action"].ToString();
                childs.Add(page);
            }

            log?.INFO($"Getting children of page:' {aemPage.Title}' successfully completed");

            return childs;
        }

        public string GetPagePath(Response response, ILogger log)
        {
            try
            {
                log?.DEBUG("Get page path");
                var doc = XDocument.Parse(response.Content);
                string path = doc.XPathSelectElement("//div[@id='Path']").Value;
                log?.DEBUG("Got page path");
                return path;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Can't get page path");
                throw new CommandAbortException("Can't get page path during exception", ex);
            }
        }

        [Command("Check response status")]
        public void CheckResponseStatus(Response response, string statusCode, ILogger log)
        {
            try
            {
                log?.DEBUG("Check response status");
                var doc = XDocument.Parse(response.Content);
                string status = doc.XPathSelectElement("//div[@id='Status']").Value;
                Equals(status, statusCode);
                log?.DEBUG("Response status checked");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Checking response status failed");
                throw new CommandAbortException("Checking response status failed during exception", ex);
            }
        }

        [Command("Check response status", "This command check response status with 200 code by default")]
        public void CheckResponseStatus(Response response, ILogger log)
        {
            try
            {
                log?.DEBUG("Check response status");
                var doc = XDocument.Parse(response.Content);
                string status = doc.XPathSelectElement("//div[@id='Status']").Value;
                Equals(status, "200");
                log?.DEBUG("Response status checked");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Checking response status failed");
                throw new CommandAbortException("Checking response status failed during exception", ex);
            }
        }

        [Command("Verify that children pages have specified status")]
        public void VerifyChildrenPagesStatus(ApiManager apiManager, AemPage aemPage, LandscapeConfig landscapeConfig, AemUser user, string status, ILogger log)
        {
            var failedList = new List<AemPage>();
            try
            {
                log?.INFO($"Start verification that all child pages under: '{aemPage.Title}' have status: {status}");
                var childPages = GetChildAemPages(apiManager, aemPage, landscapeConfig, user, log);

                log?.DEBUG($"Child pages count: {childPages.Count}");

                foreach (var page in childPages)
                {
                    log?.DEBUG($"Verify status of page: '{page.Title}'");
                    if (page.Status != status)
                    {
                        failedList.Add(page);
                        log?.ERROR($"Page: '{page.Title}' has status: {page.Status} but expected is: {status}");
                    }
                    else
                    {
                        log?.DEBUG($"Page: '{page.Title}' has expected status: {page.Status}");
                    }
                }
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during verification that child pages of '{aemPage.Title}' have status: {status}", ex);
                throw new CommandAbortException($"Error occurred during verification that child pages of '{aemPage.Title}' have status: {status}", ex);
            }

            if (failedList.Count > 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Not all child pages have expected status: {status}");
                sb.AppendLine($"List of failed pages:");
                foreach (var page in failedList)
                {
                    sb.AppendLine($"Page title: '{page.Title}', Page path: '{page.Path}', Page template: '{page.Template}', Page status: '{page.Status}'");
                }
                log?.ERROR(sb.ToString());
                throw new CommandAbortException(sb.ToString());
            }

            log?.INFO($"All child pages have expected status: {status}");
        }
    }
}
