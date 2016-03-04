namespace SapAutomation.AemUI.Components.Facets
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("Facets manager config")]
    public class FacetsManagerConfig : BaseMetaObject
    {
        [MetaTypeObject("Facets component")]
        public WebElement FacetsComponent;
    }
}
