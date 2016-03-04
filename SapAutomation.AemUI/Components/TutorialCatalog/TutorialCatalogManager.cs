namespace SapAutomation.AemUI.Components.TutorialCatalog
{
    using ContainerFinder;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [CommandManager(typeof(TutorialCatalogManagerConfig), "Tutorial catalog manager")]
    public class TutorialCatalogManager : ICommandManager
    {
        public ContainerFinderManager ContainerFinderManager { get; set; }

        public WebElement TutorialCatalogComponent;

        public TutorialCatalogManager(TutorialCatalogManagerConfig config)
        {
            TutorialCatalogComponent = config.TutorialCatalogComponent;
            ContainerFinderManager = new ContainerFinderManager(config.ContainerFinderManagerConfig);
            TutorialCatalogComponent.ChildWebElements.Add(config.ContainerFinderManagerConfig.ContainerFinderComponent);
        }

        [Command("Command for open insert dialog for drop", "OpenInsertComponentDialogForContainerFinder")]
        public void OpenInsertComponentDialogForContainerFinder(WebDriverManager wdm, ILogger log)
        {
            wdm.ActionsDoubleClick(TutorialCatalogComponent[$"TutorialCatalogPage.DragContainerFinder"], log);
        }

        [Command("Command for setup tutorial catalog", "SetUpTutorialCatalog")]
        public void SetUpTutorialCatalog(WebDriverManager wdm, TutorialSetupConfig config, ILogger log)
        {
            wdm.Click(TutorialCatalogComponent["Root.EditTutorialCatalog"], log);

            if (!string.IsNullOrEmpty(config.TutorialCardPath))
            {
                wdm.SendKeys(TutorialCatalogComponent["Root.TutorialCatalogEditor.TutorialCardsPath"], config.TutorialCardPath, log);
            }

            if (config.HideFacetsWithoutResults)
            {
                wdm.CheckCheckbox(TutorialCatalogComponent["Root.TutorialCatalogEditor.HideFacets"], log);
            }
            else
            {
                wdm.UnCheckCheckbox(TutorialCatalogComponent["Root.TutorialCatalogEditor.HideFacets"], log);
            }

            if (config.ExternalSource)
            {
                wdm.CheckCheckbox(TutorialCatalogComponent["Root.TutorialCatalogEditor.ExternalSourceCheckbox"], log);
            }
            else
            {
                wdm.UnCheckCheckbox(TutorialCatalogComponent["Root.TutorialCatalogEditor.ExternalSourceCheckbox"], log);
            }

            wdm.Click(TutorialCatalogComponent["Root.TutorialCatalogEditor.ExternalSourceCheckbox"], log);
            wdm.Click(TutorialCatalogComponent["Root.TutorialCatalogEditor.EditorOK"], log);
        }

        [Command("Command for setup tutorial catalog", "SetUpTutorialCatalog")]
        public void SetUpTutorialCatalog(WebDriverManager wdm, string value, ILogger log)
        {
            wdm.Click(TutorialCatalogComponent["Root.EditTutorialCatalog"], log);
            wdm.SendKeys(TutorialCatalogComponent["Root.TutorialCatalogEditor.TutorialCardsPath"], value, log);
            wdm.Click(TutorialCatalogComponent["Root.TutorialCatalogEditor.ExternalSourceCheckbox"], log);
            wdm.Click(TutorialCatalogComponent["Root.TutorialCatalogEditor.EditorOK"], log);
        }
    }
}
