namespace SapAutomation.AemUI.Components.ContainerFinder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Facets;
    using QA.AutomatedMagic.WebDriverManager;
    using FinderResults;

    public class ContainerFinderManagerConfig
    {
        public WebElement ContainerFinderComponent;

        public FacetsManagerConfig FacetsManagerConfig { get; set; }

        public FinderResultsManagerConfig FinderResultsManagerConfig { get; set; }
    }
}
