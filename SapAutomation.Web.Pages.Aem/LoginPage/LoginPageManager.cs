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
    using SapAutomation.Managers.AemUserManager;

    [CommandManager("Login page manager")]
    public class LoginPageManager : BaseCommandManager
    {
        [MetaSource(nameof(LoginPage) + @"\LoginPageWebDefenition.xml")]
        public WebElement LoginPageWebDefenition { get; set; }

        [Command("Login to AEM", "Login")]
        public void Login(WebDriverManager webDriverManager, AemUser user, ILogger log)
        {
            webDriverManager.SendKeys(LoginPageWebDefenition["LoginPage.UserName"], user.LoginID, log);
            Thread.Sleep(500);
            webDriverManager.SendKeys(LoginPageWebDefenition["LoginPage.Password"], user.Password, log);
            webDriverManager.Click(LoginPageWebDefenition["LoginPage.SignIn"], log);
        }
    }
}
