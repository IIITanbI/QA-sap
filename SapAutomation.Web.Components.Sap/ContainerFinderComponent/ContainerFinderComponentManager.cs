namespace SapAutomation.Web.Components.Sap.ContainerFinderComponent
{
    using Aem.InsertNewComponentForm;
    using FacetsComponent;
    using FinderResultsComponent;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.Managers.WebDriverManager;
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

        [Command("Command for setup container finder")]
        public void SetUpContainerFinder(WebDriverManager webDriverManager, ContainerFinderComponentConfig containerFinderComponentConfig, ILogger log)
        {
            log?.INFO($"Start to setup container finder component");
            log?.USEFULL($"Setuping container finder component on page: '{webDriverManager.GetCurrentUrl()}'");

            try
            {
                webDriverManager.Click(ContainerFinderComponentWebDefinition["EditContainerFinder_Button"], log);

                var containerFinderEditor = ContainerFinderComponentWebDefinition["ContainerFinderEdit_Form"];

                webDriverManager.Click(containerFinderEditor["PathConfiguration_Tab"], log);

                foreach (var path in containerFinderComponentConfig.PagePaths)
                {
                    webDriverManager.Click(containerFinderEditor["CurrentSelected_Tab.PathAdd_Button"], log);
                    webDriverManager.SendChars(containerFinderEditor["CurrentSelected_Tab.Path_Input"], path, log);
                }
                webDriverManager.Click(containerFinderEditor["EditorOK_Button"], log);

                Thread.Sleep(5000);
                log?.INFO($"Setup container finder completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during setuping container finder component");
                throw new DevelopmentException("Error occurred during setuping container finder component", ex,
                    $"Setuping container finder component on page: '{webDriverManager.GetCurrentUrl()}'");
            }
        }

        [Command("Command for add facets component")]
        public void AddFacetsComponent(WebDriverManager webDriverManager, InsertNewComponentFormManager insertNewComponentFormManager, ILogger log)
        {
            log?.INFO($"Start to add facets component");
            log?.USEFULL($"Setuping facets component on page: '{webDriverManager.GetCurrentUrl()}'");

            try
            {
                webDriverManager.ActionsDoubleClick(ContainerFinderComponentWebDefinition["DragFacets_Place"], log);
                insertNewComponentFormManager.AddComponent(webDriverManager, "FacetsComponent", log);

                log?.INFO($"Add facets component completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during setuping facets component");
                throw new DevelopmentException("Error occurred during setuping facets component", ex,
                    $"Setuping facets component on page: '{webDriverManager.GetCurrentUrl()}'");
            }
        }

        [Command("Command for add finder results component")]
        public void AddFinderResultsComponent(WebDriverManager webDriverManager, InsertNewComponentFormManager insertNewComponentFormManager, ILogger log)
        {
            log?.INFO($"Start to add finder results component");
            log?.USEFULL($"Setuping finder results component on page: '{webDriverManager.GetCurrentUrl()}'");

            try
            {
                webDriverManager.ActionsDoubleClick(ContainerFinderComponentWebDefinition["DragFinderResult_Place"], log);
                insertNewComponentFormManager.AddComponent(webDriverManager, "FinderResultComponent", log);

                log?.INFO($"Setuping finder results component completed");
            }
            catch(Exception ex)
            {
                log?.ERROR("Error occurred during setuping finder results component");
                throw new DevelopmentException("Error occurred during setuping finder results component", ex,
                    $"Setuping finder results component on page: '{webDriverManager.GetCurrentUrl()}'");
            }
        }
    }
}
