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
    using QA.AutomatedMagic.CommandsMagic;

    [CommandManager(typeof(ContainerFinderManagerConfig), "Container finder manager")]
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

        [Command("Command for setup container finder", "SetUpContainerFinder")]
        public void SetUpContainerFinder(WebDriverManager wdm, string value, ILogger log)
        {
            wdm.Click(ContainerFinderComponent["ContainerFinder.EditContainerFinder"], log);
            wdm.Click(ContainerFinderComponent["ContainerFinder.ContainerFinderEdit.PathConfiguration"], log);
            wdm.Click(ContainerFinderComponent["ContainerFinder.ContainerFinderEdit.PathAdd"], log);
            wdm.SendKeys(ContainerFinderComponent["ContainerFinder.ContainerFinderEdit.PathField"], value, log);
            wdm.Click(ContainerFinderComponent["ContainerFinder.ContainerFinderEdit.EditorOK"], log);
        }

        [Command("Command for setup container finder", "SetUpContainerFinder")]
        public void SetUpContainerFinder(WebDriverManager wdm, ContainerFinderSetupConfig config, ILogger log)
        {
            wdm.Click(ContainerFinderComponent["ContainerFinder.EditContainerFinder"], log);
            wdm.Click(ContainerFinderComponent["ContainerFinder.ContainerFinderEdit.PathConfiguration"], log);

            foreach(var path in config.Paths)
            {
                wdm.Click(ContainerFinderComponent["ContainerFinder.ContainerFinderEdit.PathAdd"], log);
                wdm.SendKeys(ContainerFinderComponent["ContainerFinder.ContainerFinderEdit.PathField"], path, log);
            }
            wdm.Click(ContainerFinderComponent["ContainerFinder.ContainerFinderEdit.EditorOK"], log);
        }


        [Command("Command for open Container Finder insert dialog for drag", "OpenContainerFinderInsertDialog")]
        public void OpenContainerFinderInsertDialog(WebDriverManager wdm, string drag, ILogger log)
        {
            wdm.ActionsDoubleClick(ContainerFinderComponent[$"ContainerFinderPage.{drag}"], log);
        }
    }
}
