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
    }
}
