namespace SapAutomation.Web.Pages.Aem.LoginPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using QA.AutomatedMagic.WebDriverManager;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic;

    [CommandManager("Login page manager")]
    public class LoginPageManager : BaseCommandManager
    {
        [MetaSource(nameof(LoginPage) + @"\LoginPageWebDefenition.xml")]
        public WebElement LoginPageWebDefenition { get; set; }

        [Command("Login to AEM", "Login")]
        public void Login(WebDriverManager webDriverManager, string userName, string password, ILogger log)
        {
            webDriverManager.SendKeys(LoginPageWebDefenition["LoginPage.UserName"], userName, log);
            webDriverManager.SendKeys(LoginPageWebDefenition["LoginPage.Password"], password, log);
            webDriverManager.Click(LoginPageWebDefenition["LoginPage.SignIn"], log);
        }
    }
}
