﻿namespace SapAutomation.Managers.AemPageManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.ApiManager;
    using QA.AutomatedMagic.WebDriverManager;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System;
    [CommandManager("Manager for aem pages")]
    public class AemPageManager : BaseCommandManager
    {
        [Command("Create AEM page", "CreatePage")]
        public void CreatePage(ApiManager apiManager, AemPage aemPage, LandscapeConfig landscapeConfig, ILogger log)
        {
            log?.INFO($"Create page with title:' {aemPage.Title}'");

            var cmd = $"/bin/wcmcommand?cmd=createPage&parentPath={aemPage.ParentPath}&title={aemPage.Title}&template={aemPage.Template}";

            log?.TRACE($"Command for page creation: {cmd}");

            var request = new Request
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = cmd
            };

            apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, log);

            log?.INFO($"Page with title:' {aemPage.Title}' successfully created");
        }

        [Command("Activate AEM page", "ActivatePage")]
        public void ActivatePage(ApiManager apiManager, AemPage aemPage, LandscapeConfig landscapeConfig, ILogger log)
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

            var response = apiManager.PerformRequest(landscapeConfig.AuthorHostUrl, request, log);

            CheckResponseStatus(response, log);
            aemPage.Path = GetPagePath(response, log);

            log?.DEBUG($"Generating command for aem page '{aemPage.Title}' activation completed");
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
            webDriverManager.Navigate($"{landscapeConfig.PublisHostUrl}/{aemPage.Title}.html", log);
            log?.DEBUG($"Opening AEM page '{aemPage.Title}' on publish completed");
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
