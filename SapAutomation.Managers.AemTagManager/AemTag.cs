namespace SapAutomation.Managers.AemTagManager
{
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("Tag")]
    public class AemTag : BaseMetaObject
    {
        [MetaTypeValue("Tag name", IsRequired = false)]
        public string Name { get; set; }

        [MetaTypeValue("Tag title", IsRequired = false)]
        public string Title { get; set; }

        [MetaTypeValue("Tag description", IsRequired = false)]
        public string Description { get; set; } = null;

        [MetaTypeValue("Tag path", IsRequired = false)]
        public string Path { get; set; } = null;

        [MetaTypeValue("Tag ID", IsRequired = false)]
        public string TagID { get; set; } = null;

        [MetaTypeValue("Tag status", IsRequired = false)]
        public string Status { get; set; } = null;
    }
}
