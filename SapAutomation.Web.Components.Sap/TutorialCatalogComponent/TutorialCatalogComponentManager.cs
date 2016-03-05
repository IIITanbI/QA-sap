namespace SapAutomation.Web.Components.Sap.TutorialCatalogComponent
{
    using ContainerFinderComponent;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.WebDriverManager;
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

        [MetaSource(nameof(TutorialCatalogComponent) + @"\TutorialCatalogComponentWebDefenition.xml")]
        public WebElement TutorialCatalogComponentWebDefinition { get; set; }

        public TutorialCatalogComponentManager()
        {
            ContainerFinderManager = AutomatedMagicManager.CreateCommandManager<ContainerFinderComponentManager>();
        }

        public override void Init()
        {
            TutorialCatalogComponentWebDefinition.ChildWebElements.Add(ContainerFinderManager.ContainerFinderComponentWebDefinition);
        }


        [Command("Command for add container finder component", "AddContainerFinderComponent")]
        public void AddContainerFinderComponent(WebDriverManager wdm, InsertNewComponentFormManager insertNewComponentFormManager, ILogger log)
        {
            wdm.ActionsDoubleClick(TutorialCatalogComponentWebDefinition[$"TutorialCatalogPage.DragContainerFinder"], log);
            insertNewComponentFormManager.AddComponent(wdm, "ContainerFinderComponent", log);
        }

        [Command("Command for setup tutorial catalog", "SetUpTutorialCatalog")]
        public void SetUpTutorialCatalog(WebDriverManager wdm, TutorialCatalogComponentConfig tutorialCatalogComponentConfig, ILogger log)
        {
            wdm.WaitForPageLoaded(log);
            wdm.WaitForJQueryLoaded(log);

            wdm.Click(TutorialCatalogComponentWebDefinition["Root.EditTutorialCatalog"], log);

            if (!string.IsNullOrEmpty(tutorialCatalogComponentConfig.TutorialCardPath))
            {
                wdm.SendKeys(TutorialCatalogComponentWebDefinition["Root.TutorialCatalogEditor.TutorialCardsPath"], tutorialCatalogComponentConfig.TutorialCardPath, log);
            }

            if (tutorialCatalogComponentConfig.HideFacetsWithoutResults)
            {
                wdm.CheckCheckbox(TutorialCatalogComponentWebDefinition["Root.TutorialCatalogEditor.HideFacets"], log);
            }
            else
            {
                wdm.UnCheckCheckbox(TutorialCatalogComponentWebDefinition["Root.TutorialCatalogEditor.HideFacets"], log);
            }

            if (tutorialCatalogComponentConfig.ExternalSource)
            {
                wdm.CheckCheckbox(TutorialCatalogComponentWebDefinition["Root.TutorialCatalogEditor.ExternalSourceCheckbox"], log);
            }
            else
            {
                wdm.UnCheckCheckbox(TutorialCatalogComponentWebDefinition["Root.TutorialCatalogEditor.ExternalSourceCheckbox"], log);
            }

            wdm.Click(TutorialCatalogComponentWebDefinition["Root.TutorialCatalogEditor.EditorOK"], log);

            wdm.Refresh(log);
            wdm.WaitForCompletelyPageLoaded(log);
        }
    }
}
