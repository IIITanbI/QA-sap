namespace SapAutomation.Web.Pages.Sap.TutorialCatalogPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Threading;
    using Components.Sap.TutorialCatalogComponent;
    using QA.AutomatedMagic.WebDriverManager;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic;

    [CommandManager("Tutorial catalog page manager")]
    public class TutorialCatalogPageManager : BaseCommandManager
    {
        public TutorialCatalogComponentManager TutorialCatalogManager { get; set; }

        [MetaSource(nameof(TutorialCatalogPage) + @"\TutorialCatalogPageWebDefenition.xml")]
        public WebElement TutorialCatalogPageWebDefenition { get; set; }

        public TutorialCatalogPageManager()
        {
            TutorialCatalogManager = AutomatedMagicManager.CreateCommandManager<TutorialCatalogComponentManager>();
        }

        public override void Init()
        {
            TutorialCatalogPageWebDefenition.ChildWebElements.Add(TutorialCatalogManager.TutorialCatalogComponentWebDefinition);
            TutorialCatalogPageWebDefenition.Init();
        }
    }
}
