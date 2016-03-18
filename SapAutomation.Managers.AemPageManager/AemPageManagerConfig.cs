namespace SapAutomation.Managers.AemPageManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("AemPageManager config")]
    public class AemPageManagerConfig : BaseMetaObject
    {
        [MetaTypeValue("Interval in seconds to perform page status checking requests", IsRequired = false)]
        public int StatusWaitInterval { get; set; } = 10;

        [MetaTypeValue("Timeout in seconds to perform page status checking", IsRequired = false)]
        public int StatusWaitTimeout { get; set; } = 300;
    }
}
