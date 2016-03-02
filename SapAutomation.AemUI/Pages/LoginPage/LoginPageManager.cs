namespace SapAutomation.AemUI.Pages.LoginPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using Components.TutorialCatalog;
    using QA.AutomatedMagic.WebDriverManager;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic;
    using AemComponents.InsertNewComponent;

    [CommandManager(typeof(LoginPageManagerConfig), "Login page manager")]
    public class LoginPageManager : ICommandManager
    {
        public WebElement LoginComponent;

        
        public LoginPageManager(LoginPageManagerConfig config)
        {
            //RootFrame = config.RootFrame;
            LoginComponent = config.LoginComponent;
        }

        public void Login(WebDriverManager wdm, string userName, string password, ILogger log)
        {
            wdm.SendKeys(LoginComponent["LoginPage.UserName"], userName, log);
            wdm.SendKeys(LoginComponent["LoginPage.Password"], password, log);
            wdm.Click(LoginComponent["LoginPage.SignIn"], log);
            
        }
    }
}
