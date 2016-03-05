namespace SapAutomation.Web.CustomComponents.TutorialCatalogComponent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic;

    [MetaType("Tutorial Catalog component config")]
    public class TutorialCatalogComponentConfig : BaseNamedMetaObject
    {
        [MetaTypeValue("Path for tutorial card", IsRequired = false)]
        public string TutorialCardPath { get; set; } = null;

        [MetaTypeValue("Is Hide Facets?", IsRequired = false)]
        public bool HideFacetsWithoutResults { get; set; }

        [MetaTypeValue("Is External Source?", IsRequired = false)]
        public bool ExternalSource { get; set; }
    }

}
