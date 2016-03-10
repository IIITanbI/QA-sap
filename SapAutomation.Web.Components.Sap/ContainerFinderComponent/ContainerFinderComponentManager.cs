namespace SapAutomation.Web.Components.Sap.ContainerFinderComponent
{
    using Aem.InsertNewComponentForm;
    using FacetsComponent;
    using FinderResultsComponent;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [CommandManager("Container finder manager")]
    public class ContainerFinderComponentManager : BaseCommandManager
    {
        public FinderResultsComponentManager FinderResultsManager { get; set; }
        public FacetsComponentManager FacetsManager { get; set; }

        [MetaSource(nameof(ContainerFinderComponent) + @"\ContainerFinderComponentWebDefinition.xml")]
        public WebElement ContainerFinderComponentWebDefinition { get; set; }

        public ContainerFinderComponentManager()
        {
            FinderResultsManager = AutomatedMagicManager.CreateCommandManager<FinderResultsComponentManager>();
            FacetsManager = AutomatedMagicManager.CreateCommandManager<FacetsComponentManager>();
        }

        public override void Init()
        {
            ContainerFinderComponentWebDefinition.ChildWebElements.Add(FinderResultsManager.FinderResultsComponentWebDefinition);
            ContainerFinderComponentWebDefinition.ChildWebElements.Add(FacetsManager.FacetsComponentWebDefinition);
        }

        [Command("Command for setup container finder", "SetUpContainerFinder")]
        public void SetUpContainerFinder(WebDriverManager webDriverManager, ContainerFinderComponentConfig containerFinderComponentConfig, ILogger log)
        {
            webDriverManager.Click(ContainerFinderComponentWebDefinition["ContainerFinder.EditContainerFinder"], log);
            webDriverManager.Click(ContainerFinderComponentWebDefinition["ContainerFinder.ContainerFinderEdit.PathConfiguration"], log);

            foreach (var path in containerFinderComponentConfig.PagePaths)
            {
                webDriverManager.Click(ContainerFinderComponentWebDefinition["ContainerFinder.ContainerFinderEdit.PathAdd"], log);
                webDriverManager.SendKeys(ContainerFinderComponentWebDefinition["ContainerFinder.ContainerFinderEdit.PathField"], path, log);
                webDriverManager.Click(ContainerFinderComponentWebDefinition["ContainerFinder.ContainerFinderEdit.PathConfiguration"], log);
            }
            webDriverManager.Click(ContainerFinderComponentWebDefinition["ContainerFinder.ContainerFinderEdit.EditorOK"], log);

            Thread.Sleep(5000);
        }

        [Command("Command for add facets component", "AddFacetsComponent")]
        public void AddFacetsComponent(WebDriverManager webDriverManager, InsertNewComponentFormManager insertNewComponentFormManager, ILogger log)
        {
            webDriverManager.ActionsDoubleClick(ContainerFinderComponentWebDefinition[$"ContainerFinderPage.DragFacets"], log);
            insertNewComponentFormManager.AddComponent(webDriverManager, "FacetsComponent", log);
        }

        [Command("Command for add finder results component", "AddFinderResultsComponent")]
        public void AddFinderResultsComponent(WebDriverManager webDriverManager, InsertNewComponentFormManager insertNewComponentFormManager, ILogger log)
        {
            webDriverManager.ActionsDoubleClick(ContainerFinderComponentWebDefinition[$"ContainerFinderPage.DragFinderResult"], log);
            insertNewComponentFormManager.AddComponent(webDriverManager, "FinderResultComponent", log);
        }
    }
}
