namespace SapAutomation.AemUI.Pages.LoginPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Components.TutorialCatalog;
    using QA.AutomatedMagic.WebDriverManager;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using AemComponents.InsertNewComponent;

    [MetaType("Login page manager config")]
    public class LoginPageManagerConfig : BaseMetaObject
    {
        [MetaTypeObject("Login page definition")]
        public WebElement LoginPageDefinition;
    }
}
