namespace SapAutomation.AemUI.Components.TutorialCatalog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.WebDriverManager;
    using ContainerFinder;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("Tutorial catalog manager config")]
    public class TutorialCatalogManagerConfig
    {
        [MetaTypeObject("Tutorial catalog component")]
        public WebElement TutorialCatalogComponent;

        [MetaTypeObject("Container finder manager config")]
        public ContainerFinderManagerConfig ContainerFinderManagerConfig { get; set; }
    }
}
