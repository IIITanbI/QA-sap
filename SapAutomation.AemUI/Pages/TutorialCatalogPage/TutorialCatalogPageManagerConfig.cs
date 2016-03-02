namespace SapAutomation.AemUI.Pages.TutorialCatalogPage
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

    [MetaType("Tutorial catalog page manager config")]
    public class TutorialCatalogPageManagerConfig : BaseMetaObject
    {
        [MetaTypeObject("Tutorial catalog root frame")]
        public WebElement RootFrame { get; set; }

        [MetaTypeObject("Tutorial catalog manager config")]
        public TutorialCatalogManagerConfig TutorialCatalogManagerConfig { get; set; }

        [MetaTypeObject("Add component form manager config")]
        public InsertNewComponentManagerConfig AddComponentFormManagerConfig { get; set; }
    }
}
