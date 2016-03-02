namespace SapAutomation.AemUI.Components.TutorialCatalog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.WebDriverManager;
    using ContainerFinder;

    public class TutorialCatalogManagerConfig
    {
        public WebElement TutorialCatalogComponent;

        public ContainerFinderManagerConfig ContainerFinderManagerConfig { get; set; }
    }
}
