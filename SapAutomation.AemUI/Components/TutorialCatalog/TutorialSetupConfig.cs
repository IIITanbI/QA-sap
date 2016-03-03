namespace SapAutomation.AemUI.Components.TutorialCatalog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("Setup for Tutorial Catalog")]
    public class TutorialSetupConfig
    {
        [MetaTypeValue("Path for tutorial card")]
        public string TutorialCardPath { get; set; }

        [MetaTypeValue("Is Hide Facets")]
        public bool HideFacetsWithoutResults { get; set; }

        [MetaTypeValue("Is External Source")]
        public bool ExternalSource { get; set; }
    }

}
