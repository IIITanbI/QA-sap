namespace SapAutomation.AemUI.Components.Facets
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [CommandManager(typeof(FacetsManagerConfig), "Facets manager")]
    public class FacetsManager : ICommandManager
    {
        public WebElement FacetsComponent;


        public FacetsManager(FacetsManagerConfig config)
        {
            FacetsComponent = config.FacetsComponent;
        }

        [Command("Command for setup facets", "SetUpFacets")]
        public void SetUpFacets(WebDriverManager wdm, FacetsSetupConfig config,  ILogger log)
        {
            wdm.Click(FacetsComponent["FacetsElement.FacetsElementEditButton"], log);

            foreach(var nameSpace in config.Namespaces)
            {
                wdm.Click(FacetsComponent["FacetsElement.FacetsElementEditor.AddNamespaces"], log);
                wdm.SendKeys(FacetsComponent["FacetsElement.FacetsElementEditor.LastPath"], nameSpace, log);
                wdm.SendKeys(FacetsComponent["FacetsElement.FacetsElementEditor.LastDefaultValue"], "1", log);
            }
            if (config.HideFacets)
            {
                wdm.CheckCheckbox(FacetsComponent["FacetsElement.FacetsElementEditor.HideFacets"], log);
            }
            else
            {
                wdm.UnCheckCheckbox(FacetsComponent["FacetsElement.FacetsElementEditor.HideFacets"], log);
            }
            wdm.Click(FacetsComponent[$"FacetsElement.FacetsElementEditor.TypeOfSelection.{config.TypeOfSelection.ToString()}"], log);
            wdm.Click(FacetsComponent["FacetsElement.FacetsElementEditor.EditorOK"], log);
        }

        [Command("Command for setup facets", "SetUpFacets")]
        public void SetUpFacets(WebDriverManager wdm, string path, string value, ILogger log)
        {
            wdm.Click(FacetsComponent["FacetsElement.FacetsElementEditButton"], log);
            wdm.Click(FacetsComponent["FacetsElement.FacetsElementEditor.HideFacets"], log);
            wdm.Click(FacetsComponent["FacetsElement.FacetsElementEditor.TypeOfSelection.List"], log);
            wdm.Click(FacetsComponent["FacetsElement.FacetsElementEditor.AddNamespaces"], log);
            wdm.SendKeys(FacetsComponent["FacetsElement.FacetsElementEditor.LastPath"], path, log);
            wdm.SendKeys(FacetsComponent["FacetsElement.FacetsElementEditor.LastDefaultValue"], value, log);
            wdm.Click(FacetsComponent["FacetsElement.FacetsElementEditor.EditorOK"], log);
        }
    }
}
