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
        [MetaTypeCollection("Analytics objects list")]
        public List<AnalyticsObject> AnalyticsObjectsList { get; set; } = new List<AnalyticsObject>();

        [MetaTypeValue("Description")]
        public string Description { get; set; }
    }
}
