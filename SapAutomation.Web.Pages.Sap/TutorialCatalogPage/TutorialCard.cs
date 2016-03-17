namespace SapAutomation.Web.Pages.Sap.TutorialCatalogPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("Tutorial card")]
    public class TutorialCard : BaseMetaObject
    {
        [MetaTypeValue("Tutorial card title", IsRequired = false)]
        public string Title { get; set; } = null;

        [MetaTypeValue("Tutorial card description", IsRequired = false)]
        public string Description { get; set; } = null;

        [MetaTypeValue("Tutorial card url", IsRequired = false)]
        public string URL { get; set; } = null;

        [MetaTypeCollection("Tutorial card tags", IsRequired = false)]
        public List<string> Tags { get; set; } = null;

        [MetaTypeValue("Tutorial card status", IsRequired = false)]
        public string Status { get; set; } = null;

        public string Name { get; set; } = null;
    }
}
