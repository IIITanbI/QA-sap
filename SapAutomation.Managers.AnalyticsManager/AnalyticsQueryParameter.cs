namespace SapAutomation.Managers.AnalyticsManager
{
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("AnalyticsQueryParameter")]
    public abstract class AnalyticsQueryParameter : BaseMetaObject
    {
        [MetaTypeValue("Analytics query parameter name")]
        public string Name { get; set; }

        [MetaTypeValue("Analytics query parameter reason", IsRequired = false)]
        public string Reason { get; set; } = null;

        [MetaTypeValue("Analytics query parameter actual value", IsRequired = false)]
        public string ActualValue { get; set; } = null;

        public abstract void Check();
    }
}
