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

        [Command("Command for newsletter setup")]
        public void SetUpNewsletter(WebDriverManager webDriverManager, NewsletterPageManagerConfig config, ILogger log)
        {
            log?.INFO("Start newsletter setup");

            try
            {
                if (config.OfficeLocation != OfficeLocations.NONE)
                    webDriverManager.Select(NewsletterPageWebDefinition["OfficeLocationDropdown"], NewsletterPageWebDefinition["OfficeLocationDropdown.OfficeLocationDropdownOption"], config.OfficeLocation.ToString(), log);

                if (config.Email != null)
                    webDriverManager.SendKeys(NewsletterPageWebDefinition["EmailField"], config.Email, log);
                webDriverManager.CheckCheckbox(NewsletterPageWebDefinition["PrivacyStatementCheckbox"],log);
                webDriverManager.Click(NewsletterPageWebDefinition["SignUpNowButton"], log);

                log?.INFO("Newsletter setup successfully completed");
            }
            catch(Exception ex)
            {
                log?.ERROR("Error occurred during newsletter setup");
                throw new DevelopmentException("Error occurred during newsletter setup", ex);
            }
        }
    }
}
