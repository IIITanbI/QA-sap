namespace SapAutomation.AemUI.Components.ContainerFinder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Facets;
    using QA.AutomatedMagic.WebDriverManager;
    using FinderResults;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic;

    [MetaType("Container finder manager config")]
    public class ContainerFinderManagerConfig : BaseMetaObject
    {
        [MetaTypeObject("Container finder component")]
        public WebElement ContainerFinderComponent;

        [MetaTypeObject("Facets manager config")]
        public FacetsManagerConfig FacetsManagerConfig { get; set; }

        [MetaTypeObject("Finder results manager config")]
        public FinderResultsManagerConfig FinderResultsManagerConfig { get; set; }
    }
}
