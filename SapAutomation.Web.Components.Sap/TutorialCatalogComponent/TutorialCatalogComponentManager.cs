namespace SapAutomation.Web.Components.Sap.TutorialCatalogComponent
{
    using ContainerFinderComponent;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.Managers.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Components.Aem.InsertNewComponentForm;

    [CommandManager("Tutorial catalog manager")]
    public class TutorialCatalogComponentManager : BaseCommandManager
    {
        public ContainerFinderComponentManager ContainerFinderManager { get; set; }

        [MetaSource(nameof(TutorialCatalogComponent) + @"\TutorialCatalogComponentWebDefinition.xml")]
        public WebElement TutorialCatalogComponentWebDefinition { get; set; }

        public TutorialCatalogComponentManager()
        {
            ContainerFinderManager = AutomatedMagicManager.CreateCommandManager<ContainerFinderComponentManager>();
        }

        public override void Init()
        {
            TutorialCatalogComponentWebDefinition.ChildWebElements.Add(ContainerFinderManager.ContainerFinderComponentWebDefinition);
        }

        [Command("Command for add container finder component")]
        public void AddContainerFinderComponent(WebDriverManager wdm, InsertNewComponentFormManager insertNewComponentFormManager, ILogger log)
        {
            wdm.ActionsDoubleClick(TutorialCatalogComponentWebDefinition["DragContainerFinder"], log);
            insertNewComponentFormManager.AddComponent(wdm, "ContainerFinderComponent", log);
        }

        [Command("Command for setup tutorial catalog")]
        public void SetUpTutorialCatalog(WebDriverManager wdm, TutorialCatalogComponentConfig tutorialCatalogComponentConfig, ILogger log)
        {
            wdm.WaitForPageLoaded(log);
            wdm.WaitForJQueryLoaded(log);

            wdm.Click(TutorialCatalogComponentWebDefinition["EditTutorialCatalog"], log);

            var editorElement = TutorialCatalogComponentWebDefinition["TutorialCatalogEditor"];

            if (!string.IsNullOrEmpty(tutorialCatalogComponentConfig.TutorialCardPath))
            {
                wdm.SendChars(editorElement["TutorialCardsPath"], tutorialCatalogComponentConfig.TutorialCardPath, log);
            }

            if (tutorialCatalogComponentConfig.HideFacetsWithoutResults)
            {
                wdm.CheckCheckbox(editorElement["HideFacets"], log);
            }
            else
            {
                wdm.UnCheckCheckbox(editorElement["HideFacets"], log);
            }

            if (tutorialCatalogComponentConfig.ExternalSource)
            {
                wdm.CheckCheckbox(editorElement["ExternalSourceCheckbox"], log);
            }
            else
            {
                wdm.UnCheckCheckbox(editorElement["ExternalSourceCheckbox"], log);
            }

            wdm.Click(editorElement["EditorOK"], log);

            Thread.Sleep(1000);
            wdm.Refresh(log);
            Thread.Sleep(5000);
        }
    }
}
