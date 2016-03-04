namespace SapAutomation.AemUI.Components.ContainerFinder
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("Setup for container finder element")]
    public class ContainerFinderConfig
    {
        [MetaTypeCollection("Page pathes included in Finder Search Results")]
        public List<string> Paths = new List<string>();

    }
}
