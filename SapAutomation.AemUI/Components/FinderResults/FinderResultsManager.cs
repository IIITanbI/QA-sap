namespace SapAutomation.AemUI.Components.FinderResults
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FinderResultsManager : ICommandManager
    {
        public WebElement FinderResultComponent;

        public FinderResultsManager(FinderResultsManagerConfig config)
        {
            FinderResultComponent = config.FinderResultsComponent;
        }
    }
}
