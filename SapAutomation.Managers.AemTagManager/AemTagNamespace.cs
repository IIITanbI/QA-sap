namespace SapAutomation.Managers.AemTagManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("Aem Tag Namespace")]
    public class AemTagNamespace : AemTag
    {
        public AemTagNamespace()
        {
            TagType = AemTagType.Namespace;
        }
    }
}
