namespace SapAutomation.Web.Components.Sap.FacetsComponent
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.Managers.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [CommandManager("Facets manager")]
    public class FacetsComponentManager : BaseCommandManager
    {
        [MetaSource(nameof(FacetsComponent) + @"\FacetsComponentWebDefinition.xml")]
        public WebElement FacetsComponentWebDefinition { get; set; }

        [Command("Command for setup facets")]
        public void SetUpFacets(WebDriverManager webDriverManager, FacetsComponentConfig facetsComponentConfig, ILogger log)
        {
            log?.INFO($"Start to setup facets");
            log?.USEFULL($"Setuping facets component on page: '{webDriverManager.GetCurrentUrl()}'");
            try
            {
                webDriverManager.Click(FacetsComponentWebDefinition["FacetsElementEdit_Button"], log);

                int i = 1;

                var facetsEditorElement = FacetsComponentWebDefinition["FacetsElementEditor_Form"];

                foreach (var nameSpace in facetsComponentConfig.Namespaces)
                {
                    webDriverManager.Click(facetsEditorElement["AddNamespaces_Button"], log);
                    webDriverManager.SendChars(facetsEditorElement["LastPath_Input"], nameSpace, log);
                    webDriverManager.Click(facetsEditorElement["Facets_Tab"], log);
                    webDriverManager.SendKeys(facetsEditorElement["LastDefaultValue_Input"], i.ToString(), log);
                    i++;
                }

                if (facetsComponentConfig.HideFacets)
                {
                    webDriverManager.CheckCheckbox(facetsEditorElement["HideFacets_Checkbox"], log);
                }
                else
                {
                    webDriverManager.UnCheckCheckbox(facetsEditorElement["HideFacets_Checkbox"], log);
                }


                var selectionsCheckboxes = facetsEditorElement["TypeOfSelection_Checkboxes"];
                switch (facetsComponentConfig.TypeOfSelection)
                {
                    case TypeOfSelection.CheckBoxes:
                        webDriverManager.Click(selectionsCheckboxes["CheckBoxes_RadioButton"], log);
                        break;
                    case TypeOfSelection.DropDownList:
                        webDriverManager.Click(selectionsCheckboxes["DropDownList_RadioButton"], log);
                        break;
                    case TypeOfSelection.List:
                        webDriverManager.Click(selectionsCheckboxes["List_RadioButton"], log);
                        break;
                    case TypeOfSelection.RadioButtons:
                        webDriverManager.Click(selectionsCheckboxes["RadioButtons_RadioButton"], log);
                        break;
                    default:
                        break;
                }

                webDriverManager.Click(facetsEditorElement["EditorOK_Button"], log);

                Thread.Sleep(5000);

                log?.INFO($"Setup facets completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during setuping facets component");
                throw new DevelopmentException("Error occurred during setuping facets component", ex,
                    $"Setuping facets component on page: '{webDriverManager.GetCurrentUrl()}'");
            }
        }
    }
}
