namespace SapAutomation.Managers.AemUserManager
{
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("Aem group")]
    public class AemGroup : BaseMetaObject
    {
        [MetaTypeValue("Aem group ID")]
        public string GroupID { get; set; }

        [MetaTypeValue("Aem group name", IsRequired = false)]
        public string GroupName { get; set; } = null;

        [MetaTypeValue("Aem group description", IsRequired = false)]
        public string Description { get; set; } = null;
    }
}
