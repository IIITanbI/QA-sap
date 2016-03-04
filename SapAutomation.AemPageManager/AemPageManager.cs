namespace SapAutomation.AemPageManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.ApiManager;
    using System;

    [CommandManager("Manager for aem pages")]
    public class AemPageManager : ICommandManager
    {
        [Command("Create AEM page", "CreatePage")]
        public void CreatePage(ApiManager apiManager, AemPage aemPage, ILogger log)
        {
            try
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

                apiManager.PerformRequest(request, log);

                log?.INFO($"Page with title:' {aemPage.Title}' successfully created");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during generating command for aem page '{aemPage.Title}' creation");
                throw new CommandAbortException($"Error occurred during generating command for aem page '{aemPage.Title}' creation", ex);
            }
        }

        [Command("Activate AEM page")]
        public void ActivatePage(ApiManager apiManager, AemPage aemPage, ILogger log)
        {
            try
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

                apiManager.PerformRequest(request, log);

                log?.DEBUG($"Generating command for aem page '{aemPage.Title}' activation completed");
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during generating command for aem page '{aemPage.Title}' activation");
                throw new CommandAbortException($"Error occurred during generating command for aem page '{aemPage.Title}' activation", ex);
            }
        }
    }
}
