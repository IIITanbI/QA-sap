namespace SapAutomation.AemUI.Pages.TutorialCatalogPage
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

    [CommandManager(typeof(TutorialCatalogPageManagerConfig), "Tutorial catalog page manager")]
    public class TutorialCatalogPageManager : ICommandManager
    {
        public TutorialCatalogManager TutorialCatalogManager { get; set; }

        public InsertNewComponentManager AddComponentFormManager { get; set; }

        public WebElement RootFrame;

        public TutorialCatalogPageManager(TutorialCatalogPageManagerConfig config)
        {
            RootFrame = config.RootFrame;
            TutorialCatalogManager = new TutorialCatalogManager(config.TutorialCatalogManagerConfig);
            AddComponentFormManager = new InsertNewComponentManager(config.AddComponentFormManagerConfig);
            RootFrame.ChildWebElements.Add(config.TutorialCatalogManagerConfig.TutorialCatalogComponent);
            RootFrame.ChildWebElements.Add(config.AddComponentFormManagerConfig.InsertNewComponent);
            RootFrame.Init();
        }
    }
}
