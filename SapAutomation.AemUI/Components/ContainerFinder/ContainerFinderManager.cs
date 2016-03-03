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
            wdm.Click(ContainerFinderComponent["Root.EditTutorialCatalog"], log);
            wdm.SendKeys(ContainerFinderComponent["Root.TutorialCatalogEditor.TutorialCardsPath"], value, log);
            wdm.Click(ContainerFinderComponent["Root.TutorialCatalogEditor.ExternalSourceCheckbox"], log);
            wdm.Click(ContainerFinderComponent["Root.TutorialCatalogEditor.EditorOK"], log);
        }

        [Command("Command for open insert dialog for finder result", "OpenResultInsertDialog")]
        public void OpenResultInsertDialog(WebDriverManager wdm, ILogger log)
        {
            wdm.ActionsDoubleClick(ContainerFinderComponent["ContainerFinderPage.DragFinderResult"], log);
        }
    }
}
