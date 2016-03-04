namespace SapAutomation.AemUI.Components.ContainerFinder
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic;

    [MetaType("Setup for container finder element")]
    public class ContainerFinderConfig  : BaseMetaObject
    {
        [MetaTypeCollection("Page pathes included in Finder Search Results", IsRequired = false)]
        public List<string> Paths { get; set; } = new List<string>();
    }
}
