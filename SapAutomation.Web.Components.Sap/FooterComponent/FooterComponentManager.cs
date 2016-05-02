namespace SapAutomation.Web.Components.Sap.FooterComponent
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic.Managers.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [CommandManager("Footer manager")]
    public class FooterComponentManager : BaseCommandManager
    {
        [MetaSource(nameof(FooterComponent) + @"/FooterComponentWebDefinition.xml")]
        public WebElement FooterComponentWebDefinition { get; set; }

        public override void Init()
        {
            FooterComponentWebDefinition.Init();
        }

        [Command("Command for click on Privacy link")]
        public void ClickPrivacy(WebDriverManager webDriverManager, ILogger log)
        {
            log?.INFO("Start click on Privacy");
            try
            {
                webDriverManager.Click(FooterComponentWebDefinition["DesktopLinks_Div.Privacy_Link"], log);

                log?.INFO("Click on Privacy link successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on Privacy link");
                throw new DevelopmentException("Error occurred during clicking on privacy link", ex);
            }
        }

        [Command("Command for click on Terms Of Use link")]
        public void ClickTermsOfUse(WebDriverManager webDriverManager, ILogger log)
        {
            log?.INFO("Start click on Terms Of Use link");
            try
            {
                webDriverManager.Click(FooterComponentWebDefinition["DesktopLinks_Div.TermsOfUse_Link"], log);

                log?.INFO("Click on Terms Of Use link successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on TermsOfUse link");
                throw new DevelopmentException("Error occurred during clicking on TermsOfUse link", ex);
            }
        }

        [Command("Command for click on Legal Disclosure link")]
        public void ClickLegalDisclosure(WebDriverManager webDriverManager, ILogger log)
        {
            log?.INFO("Start click on Legal Disclosure link");
            try
            {
                webDriverManager.Click(FooterComponentWebDefinition["DesktopLinks_Div.LegalDisclosure_Link"], log);

                log?.INFO("Click on Legal Disclosure successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on LegalDisclosure link");
                throw new DevelopmentException("Error occurred during clicking on LegalDisclosure link", ex);
            }
        }

        [Command("Command for click on Copyright link")]
        public void ClickCopyright(WebDriverManager webDriverManager, ILogger log)
        {
            log?.INFO("Start click on Copyright link");
            try
            {
                webDriverManager.Click(FooterComponentWebDefinition["DesktopLinks_Div.Copyright_Link"], log);

                log?.INFO("Click on Copyright link successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on Copyright link");
                throw new DevelopmentException("Error occurred during clicking on Copyright link", ex);
            }
        }

        [Command("Command for click on Trademark link")]
        public void ClickTrademark(WebDriverManager webDriverManager, ILogger log)
        {
            log?.INFO("Start click on Trademark link");
            try
            {
                webDriverManager.Click(FooterComponentWebDefinition["DesktopLinks_Div.Trademark_Link"], log);

                log?.INFO("Click on Trademark link successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on Trademark link");
                throw new DevelopmentException("Error occurred during clicking on Trademark link", ex);
            }
        }

        [Command("Command for click on Sitemap link")]
        public void ClickSitemap(WebDriverManager webDriverManager, ILogger log)
        {
            log?.INFO("Start click on Sitemap link");
            try
            {
                webDriverManager.Click(FooterComponentWebDefinition["DesktopLinks_Div.Sitemap_Link"], log);

                log?.INFO("Click on Sitemap link successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on Sitemap link");
                throw new DevelopmentException("Error occurred during clicking on Sitemap link", ex);
            }
        }

        [Command("Command for click on Newsletter link")]
        public void ClickNewsletter(WebDriverManager webDriverManager, ILogger log)
        {
            log?.INFO("Start click on Newsletter link");
            try
            {
                webDriverManager.Click(FooterComponentWebDefinition["DesktopLinks_Div.Newsletter_Link"], log);

                log?.INFO("Click on Newsletter link successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on Newsletter link");
                throw new DevelopmentException("Error occurred during clicking on Newsletter link", ex);
            }
        }

        [Command("Command for click on Text View link")]
        public void ClickTextView(WebDriverManager webDriverManager, ILogger log)
        {
            log?.INFO("Start click on Text View link");
            try
            {
                webDriverManager.JSScrollTo(FooterComponentWebDefinition["DesktopLinks_Div.TextView_Link"], log);
                webDriverManager.Click(FooterComponentWebDefinition["DesktopLinks_Div.TextView_Link"], log);

                log?.INFO("Click on Text View link successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on Text View link");
                throw new DevelopmentException("Error occurred during clicking on Text View link", ex);
            }
        }
    }
}
