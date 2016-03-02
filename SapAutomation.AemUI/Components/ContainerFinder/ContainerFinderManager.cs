namespace SapAutomation.AemUI.Components.ContainerFinder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using Facets;
    using FinderResults;
    using System.Threading;

    public class ContainerFinderManager : ICommandManager
    {
        public FinderResultsManager FinderResultsManager { get; set; }

        public FacetsManager FacetsManager { get; set; }

        public WebElement ContainerFinderComponent;

        public ContainerFinderManager(ContainerFinderManagerConfig config)
        {
            ContainerFinderComponent = config.ContainerFinderComponent;
            FinderResultsManager = new FinderResultsManager(config.FinderResultsManagerConfig);
            FacetsManager = new FacetsManager(config.FacetsManagerConfig);
            ContainerFinderComponent.ChildWebElements.Add(config.FinderResultsManagerConfig.FinderResultsComponent);
            ContainerFinderComponent.ChildWebElements.Add(config.FacetsManagerConfig.FacetsComponent);
        }
    }
}
