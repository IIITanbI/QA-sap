namespace SapAutomation.Managers.AnalyticsManager
{
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("AnalyticsTest")]
    public class AnalyticsTest : BaseNamedMetaObject
    {
        [MetaTypeCollection("Analytics query parameters list", IsAssignableTypesAllowed = true)]
        public List<AnalyticsQueryParameter> AnalyticsQueryParametersList { get; set; } = new List<AnalyticsQueryParameter>();

        [MetaTypeValue("Description")]
        public string Description { get; set; }
    }
}
