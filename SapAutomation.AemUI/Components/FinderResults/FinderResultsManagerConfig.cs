namespace SapAutomation.AemUI.Components.FinderResults
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("Finder results manager config")]
    public class FinderResultsManagerConfig : BaseMetaObject
    {
        [MetaTypeObject("Finder results component")]
        public WebElement FinderResultsComponent;
    }
}
