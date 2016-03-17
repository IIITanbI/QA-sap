namespace SapAutomation.Web.Components.Sap.FacetsComponent
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.WebDriverManager;
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
            webDriverManager.Click(FacetsComponentWebDefinition["FacetsElementEditButton"], log);

            int i = 1;

            var facetsEditorElement = FacetsComponentWebDefinition["FacetsElementEditor"];

            foreach (var nameSpace in facetsComponentConfig.Namespaces)
            {
                webDriverManager.Click(facetsEditorElement["AddNamespaces"], log);
                webDriverManager.SendChars(facetsEditorElement["LastPath"], nameSpace, log);
                webDriverManager.Click(facetsEditorElement["FacetsTab"], log);
                webDriverManager.SendKeys(facetsEditorElement["LastDefaultValue"], i.ToString(), log);
                i++;
            }

            if (facetsComponentConfig.HideFacets)
            {
                webDriverManager.CheckCheckbox(facetsEditorElement["HideFacets"], log);
            }
            else
            {
                webDriverManager.UnCheckCheckbox(facetsEditorElement["HideFacets"], log);
            }

            webDriverManager.Click(facetsEditorElement[$"TypeOfSelection.{facetsComponentConfig.TypeOfSelection.ToString()}"], log);
            webDriverManager.Click(facetsEditorElement["EditorOK"], log);

            Thread.Sleep(5000);
        }
    }
}
