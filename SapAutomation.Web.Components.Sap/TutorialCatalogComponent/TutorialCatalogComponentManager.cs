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
            ContainerFinderManager = AutomatedMagicManager.CreateCommandManager<ContainerFinderComponentManager>("TutorialCatalogComponentManager.ContainerFinderComponentManager");
        }

        public override void Init()
        {
            TutorialCatalogComponentWebDefinition.ChildWebElements.Add(ContainerFinderManager.ContainerFinderComponentWebDefinition);
        }

        [Command("Command for add container finder component")]
        public void AddContainerFinderComponent(WebDriverManager wdm, InsertNewComponentFormManager insertNewComponentFormManager, ILogger log)
        {
            log?.INFO($"Start to add container finder component");
            log?.USEFULL($"Add container finder component on page: '{wdm.GetCurrentUrl()}'");

            try
            {
                wdm.ActionsDoubleClick(TutorialCatalogComponentWebDefinition["DragContainerFinder_Place"], log);
                insertNewComponentFormManager.AddComponent(wdm, "ContainerFinderComponent", log);

                log?.INFO($"Add container finder component completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during adding container finder component");
                throw new DevelopmentException("Error occurred during adding container finder component", ex,
                    $"Adding container finder component on page: '{wdm.GetCurrentUrl()}'");
            }
        }

        [Command("Command for setup tutorial catalog")]
        public void SetUpTutorialCatalog(WebDriverManager wdm, TutorialCatalogComponentConfig tutorialCatalogComponentConfig, ILogger log)
        {
            log?.INFO($"Start to setup tutorial catalog");
            log?.USEFULL($"Setuping tutorial catalog on page: '{wdm.GetCurrentUrl()}'");

            try
            {
                wdm.WaitForPageLoaded(log);
                wdm.WaitForJQueryLoaded(log);

                wdm.Click(TutorialCatalogComponentWebDefinition["EditTutorialCatalog_Button"], log);

                var editorElement = TutorialCatalogComponentWebDefinition["TutorialCatalogEditor_Form"];

                if (!string.IsNullOrEmpty(tutorialCatalogComponentConfig.TutorialCardPath))
                {
                    wdm.SendChars(editorElement["TutorialCardsPath_Input"], tutorialCatalogComponentConfig.TutorialCardPath, log);
                }

                if (tutorialCatalogComponentConfig.HideFacetsWithoutResults)
                {
                    wdm.CheckCheckbox(editorElement["HideFacets_Checkbox"], log);
                }
                else
                {
                    wdm.UnCheckCheckbox(editorElement["HideFacets_Checkbox"], log);
                }

                if (tutorialCatalogComponentConfig.ExternalSource)
                {
                    wdm.CheckCheckbox(editorElement["ExternalSource_Checkbox"], log);
                }
                else
                {
                    wdm.UnCheckCheckbox(editorElement["ExternalSource_Checkbox"], log);
                }

                wdm.Click(editorElement["EditorOK_Button"], log);

                Thread.Sleep(1000);
                wdm.Refresh(log);
                Thread.Sleep(5000);

                log?.INFO($"Setup tutorial catalog completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during setuping tutorial catalog");
                throw new DevelopmentException("Error occurred during setuping tutorial catalog", ex,
                    $"Setuping tutorial catalog on page: '{wdm.GetCurrentUrl()}'");
            }
        }
    }
}
