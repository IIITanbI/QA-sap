namespace SapAutomation.Managers.AemPageManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.ApiManager;
    using QA.AutomatedMagic.WebDriverManager;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System;
    using AemUserManager;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using System.Linq;
    [CommandManager("Manager for aem pages")]
    public class AemPageManager : BaseCommandManager
    {
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
            var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.LoginID, user.Password, log);

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
            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.LoginID, user.Password, log);

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
            var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.LoginID, user.Password, log);

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
            var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.LoginID, user.Password, log);

            CheckResponseStatus(response, log);

            log?.DEBUG($"Generating command for aem page '{aemPage.Title}' deactivation completed");
        }

        [Command("Open AEM page on author", "OpenPageOnAuthor")]
        public void OpenPageOnAuthor(WebDriverManager webDriverManager, AemPage aemPage, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.DEBUG($"Open AEM page '{aemPage.Title}' on author");
            webDriverManager.Navigate($"{landscapeConfig.AuthorHostUrl}/cf#{aemPage.ParentPath}/{aemPage.Title.ToLower()}.html", log);
            log?.DEBUG($"Opening AEM page '{aemPage.Title}' on author completed");
        }

        [Command("Open AEM page on publish", "OpenPageOnPublish")]
        public void OpenPageOnPublish(WebDriverManager webDriverManager, AemPage aemPage, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.DEBUG($"Open AEM page '{aemPage.Title}' on publish");
            webDriverManager.Navigate($"{landscapeConfig.PublishHostUrl}/{aemPage.Title}.html", log);
            log?.DEBUG($"Opening AEM page '{aemPage.Title}' on publish completed");
        }

        [Command("Get child aem pages")]
        public List<AemPage> GetChildAemPages(ApiManager apiManager, AemPage aemPage, LandscapeConfig landscapeConfig, AemUser user, ILogger log)
        {
            List<AemPage> childs = new List<AemPage>();

            log?.INFO($"Get childs of page:' {aemPage.Title}'");

            var cmd = $"/bin/wcm/siteadmin/tree.json?path={aemPage.ParentPath}/{aemPage.Title.ToLower()}";

            var request = new Request
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.GET,
                PostData = cmd
            };

            var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, user.LoginID, user.Password, log);
            var tmp = JArray.Parse(response.Content);

            foreach (var child in tmp)
            {
                var page = new AemPage();
                page.Title = child["name"].ToString();
                page.ParentPath = $"{aemPage.ParentPath}/{aemPage.Title.ToLower()}";
                page.Path = $"{aemPage.ParentPath}/{aemPage.Title.ToLower()}/{child["name"].ToString()}";
                if (child["replication"].Children().Count() != 0)
                    page.Status = child["replication"]["action"].ToString();
                childs.Add(page);
            }

            log?.INFO($"GEtting childs of page:' {aemPage.Title}' successfully completed");

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
    }
}
