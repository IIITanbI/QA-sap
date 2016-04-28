namespace SapAutomation
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("Config for landscape")]
    public class LandscapeConfig : BaseNamedMetaObject
    {
        [MetaTypeValue("Author host url")]
        public string AuthorHostUrl { get; set; }

        [MetaTypeValue("Publish host url")]
        public string PublishHostUrl { get; set; }

        [MetaTypeValue("SolrSearchLink")]
        public string SolrSearchLink { get; set; }

        [MetaTypeValue("Is production?")]
        public bool IsProduction { get; set; }
    }
}
