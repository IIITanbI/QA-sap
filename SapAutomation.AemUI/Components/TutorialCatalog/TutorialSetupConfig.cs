﻿namespace SapAutomation.AemUI.Components.TutorialCatalog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic;

    [MetaType("Setup for Tutorial Catalog")]
    public class TutorialSetupConfig : BaseMetaObject
    {
        [MetaTypeValue("Path for tutorial card", IsRequired = false)]
        public string TutorialCardPath { get; set; } = null;

        [MetaTypeValue("Is Hide Facets", IsRequired = false)]
        public bool HideFacetsWithoutResults { get; set; }

        [MetaTypeValue("Is External Source", IsRequired = false)]
        public bool ExternalSource { get; set; }
    }

}