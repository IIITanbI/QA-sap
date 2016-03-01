namespace SapAutomation.AemPageManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.ApiManager;
    using System;

    [CommandManager("Manager for aem pages")]
    public class AemPageManager : ICommandManager
    {
        [Command("Create page", "CreatePage")]
        public void CreatePage(ApiManager apiManager, AemPage aemPage, ILogger log)
        {
            try
            {
                log?.DEBUG($"Create page with title:' {aemPage.Title}'");

                var cmd = $"?cmd=createPage&parentPath={aemPage.ParentPath}&title={aemPage.Title}&template={aemPage.Template}";

                log?.TRACE($"Command for page creation: {cmd}");

                var request = new Request
                {
                    ContentType = "text/html;charset=UTF-8",
                    Method = Request.Methods.POST,
                    PostData = cmd
                };

                apiManager.PerformRequest(request, log);
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during generating command for aem page '{aemPage.Title}' creation");
                throw new CommandAbortException($"Error occurred during generating command for aem page '{aemPage.Title}' creation", ex);
            }
        }

        [Command("Generate command for aem page activation", "ActivatePageCmd")]
        public string GenerateCommandForActivatePage(AemPage aemPage, ILogger log)
        {
            try
            {
                log?.DEBUG($"Generate command for aem page '{aemPage.Title}' activation");

                var tmp = $"?cmd=Activate&path={aemPage.ParentPath}/{aemPage.Title}";

                log?.DEBUG($"Generating command for aem page '{aemPage.Title}' activation completed");

                return tmp;
            }
            catch (Exception ex)
            {
                log?.ERROR($"Error occurred during generating command for aem page '{aemPage.Title}' activation");
                throw new CommandAbortException($"Error occurred during generating command for aem page '{aemPage.Title}' activation", ex);
            }
        }
    }
}
