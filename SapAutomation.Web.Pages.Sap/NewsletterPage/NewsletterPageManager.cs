namespace SapAutomation.Web.Pages.Sap.NewsletterPage
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.Managers.WebDriverManager;
    using System;

    [CommandManager("NewsletterPageManager")]
    public class NewsletterPageManager : BaseCommandManager
    {
        [MetaSource(nameof(NewsletterPage) + @"\NewsletterPageWebDefinition.xml")]
        public WebElement NewsletterPageWebDefinition { get; set; }

        public override void Init()
        {
            NewsletterPageWebDefinition.Init();
        }

        [Command("Command for newsletter setup")]
        public void SetUpNewsletter(WebDriverManager webDriverManager, NewsletterPageConfig config, ILogger log)
        {
            log?.INFO("Start newsletter setup");

            try
            {
                if (config.OfficeLocation != null)
                    webDriverManager.SelectByText(NewsletterPageWebDefinition["OfficeLocation_Dropdown"], config.OfficeLocation, log);

                if (config.Email != null)
                    webDriverManager.SendKeys(NewsletterPageWebDefinition["Email_Input"], config.Email, log);

                webDriverManager.CheckCheckbox(NewsletterPageWebDefinition["PrivacyStatement_Checkbox"], log);
                webDriverManager.Click(NewsletterPageWebDefinition["SignUpNow_Button"], log);

                log?.INFO("Newsletter setup successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during newsletter setup");
                throw new DevelopmentException("Error occurred during newsletter setup", ex);
            }
        }
    }
}
