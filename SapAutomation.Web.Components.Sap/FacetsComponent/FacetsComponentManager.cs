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

        [Command("Command for setup facets", "SetUpFacets")]
        public void SetUpFacets(WebDriverManager webDriverManager, FacetsComponentConfig facetsComponentConfig, ILogger log)
        {
            webDriverManager.Click(FacetsComponentWebDefinition["FacetsElement.FacetsElementEditButton"], log);

            int i = 1;

            foreach (var nameSpace in facetsComponentConfig.Namespaces)
            {
                webDriverManager.Click(FacetsComponentWebDefinition["FacetsElement.FacetsElementEditor.AddNamespaces"], log);
                webDriverManager.SendChars(FacetsComponentWebDefinition["FacetsElement.FacetsElementEditor.LastPath"], nameSpace, log);
                webDriverManager.Click(FacetsComponentWebDefinition["FacetsElement.FacetsElementEditor.FacetsTab"], log);
                webDriverManager.SendKeys(FacetsComponentWebDefinition["FacetsElement.FacetsElementEditor.LastDefaultValue"], i.ToString(), log);
                i++;
            }

            if (facetsComponentConfig.HideFacets)
            {
                webDriverManager.CheckCheckbox(FacetsComponentWebDefinition["FacetsElement.FacetsElementEditor.HideFacets"], log);
            }
            else
            {
                webDriverManager.UnCheckCheckbox(FacetsComponentWebDefinition["FacetsElement.FacetsElementEditor.HideFacets"], log);
            }

            webDriverManager.Click(FacetsComponentWebDefinition[$"FacetsElement.FacetsElementEditor.TypeOfSelection.{facetsComponentConfig.TypeOfSelection.ToString()}"], log);
            webDriverManager.Click(FacetsComponentWebDefinition["FacetsElement.FacetsElementEditor.EditorOK"], log);

            Thread.Sleep(5000);
        }
    }
}
