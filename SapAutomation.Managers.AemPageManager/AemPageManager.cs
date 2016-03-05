namespace SapAutomation.Managers.AemPageManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.ApiManager;
    using System;

    [CommandManager("Manager for aem pages")]
    public class AemPageManager : BaseCommandManager
    {
        [Command("Create AEM page", "CreatePage")]
        public string CreatePage(ApiManager apiManager, AemPage aemPage, string host, ILogger log)
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

            apiManager.PerformRequest(host, request, log);

            log?.INFO($"Page with title:' {aemPage.Title}' successfully created");

            return host + "/cf#" + aemPage.ParentPath + "/" + aemPage.Title.ToLower() + ".html";
        }

        [Command("Activate AEM page", "ActivatePage")]
        public void ActivatePage(ApiManager apiManager, AemPage aemPage, string host, ILogger log)
        {
            log?.DEBUG($"Generate command for aem page '{aemPage.Title}' activation");

            var cmd = $"/bin/replicate.json?cmd=Activate&path={aemPage.ParentPath}/{aemPage.Title}";

            log?.TRACE($"Command for page activation: {cmd}");

            var request = new Request
            {
                ContentType = "text/html;charset=UTF-8",
                Method = Request.Methods.POST,
                PostData = cmd
            };

            apiManager.PerformRequest(host, request, log);

            log?.DEBUG($"Generating command for aem page '{aemPage.Title}' activation completed");
        }
    }
}
