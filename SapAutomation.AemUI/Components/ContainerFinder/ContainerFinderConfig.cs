using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapAutomation.AemUI.Components.ContainerFinder
{
    public class ContainerFinderConfig
    {
        public List<string> Paths { get; set; }

        public ContainerFinderConfig(params string[] paths)
        {
            Paths = new List<string>();
            Paths.AddRange(paths);
        }
    }
}
