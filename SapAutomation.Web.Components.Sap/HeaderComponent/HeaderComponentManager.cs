namespace SapAutomation.Web.Components.Sap.HeaderComponent
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.Managers.WebDriverManager;
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [CommandManager("Header manager")]
    public class HeaderComponentManager : BaseCommandManager
    {
        [MetaSource(nameof(HeaderComponent) + @"/HeaderComponentWebDefinition.xml")]
        public WebElement HeaderComponentWebDefinition { get; set; }

        public override void Init()
        {
            HeaderComponentWebDefinition.Init();
        }

        [Command("Command for click on SAP logo")]
        public void ClickLogo(WebDriverManager webDriverManager, ILogger log)
        {
            log?.INFO("Start click on SAP logo");
            try
            {
                webDriverManager.Click(HeaderComponentWebDefinition["Logo_Image"], log);

                log?.INFO("Click on SAP logo successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on SAP logo");
                throw new DevelopmentException("Error occurred during clicking on SAP logo", ex);
            }
        }

        [Command("Command for click on login icon")]
        public void ClickLogin(WebDriverManager webDriverManager, ILogger log)
        {
            log?.INFO("Start click on login icon");
            try
            {
                webDriverManager.Click(HeaderComponentWebDefinition["Login_Image"], log);

                log?.INFO("Click on login icon successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on login icon");
                throw new DevelopmentException("Error occurred during clicking on login icon", ex);
            }
        }

        [Command("Command for click on country selector icon")]
        public void ClickCountrySelector(WebDriverManager webDriverManager, ILogger log)
        {
            log?.INFO("Start click on country selector icon");
            try
            {
                webDriverManager.Click(HeaderComponentWebDefinition["CountrySelector_Image"], log);

                log?.INFO("Click on country selector icon successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on country selector icon");
                throw new DevelopmentException("Error occurred during clicking on country selector icon", ex);
            }
        }

        [Command("Command for click on country link")]
        public void ClickCountry(WebDriverManager webDriverManager, string value, ILogger log)
        {
            log?.INFO("Start click on country link");
            try
            {
                var temp = HeaderComponentWebDefinition["Country_Link"];
                var option = MetaType.CopyObjectWithCast(temp);
                option.ParentElement = option.ParentElement;
                option.Locator.XPath = option.Locator.XPath.Replace("toReplace", value);

                // @data-locale-code="fr_ca"
                webDriverManager.Click(option, log);

                log?.INFO("Click on country link successfully completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during clicking on country link");
                throw new DevelopmentException("Error occurred during clicking on country link", ex);
            }
        }
    }
}
