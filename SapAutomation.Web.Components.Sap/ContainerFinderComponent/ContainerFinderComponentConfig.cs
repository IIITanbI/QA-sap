namespace SapAutomation.Web.Components.Sap.ContainerFinderComponent
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic;

    [MetaType("Setup for container finder element")]
    public class ContainerFinderComponentConfig : BaseNamedMetaObject
    {
        [MetaTypeCollection("Page paths included in Finder Search Results", "pagePath", "path", IsRequired = false)]
        public List<string> PagePaths { get; set; } = new List<string>();
    }
}
