namespace SapAutomation.Managers.AnalyticsManager
{
    using System;
    using QA.AutomatedMagic.MetaMagic;
    using System.Collections.Generic;
    using System.Linq;

    [MetaType("AnalyticsQueryParameterEvent")]
    public class AnalyticsQueryParameterEvent : AnalyticsQueryParameter
    {
        [MetaTypeValue("Analytics query parameter value", IsRequired = false)]
        public string ExpectedValue { get; set; } = null;

        [MetaTypeCollection("Analytics query parameter list", IsAssignableTypesAllowed = true, IsRequired = false)]
        public List<AnalyticsQueryParameter> AnalyticsEventsList = new List<AnalyticsQueryParameter>();

        public override void Check()
        {
            if (ActualValue != null && ExpectedValue != null)
            {
                var events = ActualValue.Split(',').ToList();
                foreach (var aEvent in events)
                {
                    var keyValue = aEvent.Split(':');
                    var eventName = keyValue[0];
                    var analyticsEvent = AnalyticsEventsList.FirstOrDefault(ae => ae.Name == eventName);
                    if (analyticsEvent != null)
                    {
                        if (keyValue.Length == 2)
                        {
                            analyticsEvent.ActualValue = keyValue[1];
                        }
                        else
                        {
                            analyticsEvent.ActualValue = string.Empty;
                        }
                    }
                }
                foreach (var AnalyticsEvent in AnalyticsEventsList)
                {
                    AnalyticsEvent.Check();
                    if (AnalyticsEvent.Reason != null)
                    {
                        Reason += $"\n\t{AnalyticsEvent.Reason}";
                    }
                }
            }
            else if (ActualValue == null && ExpectedValue == null)
            {
                return;
            }
            else if (ActualValue == null)
            {
                Reason = $"Actual value is missing, but expected value is '{ExpectedValue}'.";
            }
            else
            {
                Reason = $"Expected value isn't specified, but actual value is '{ActualValue}'.";
            }
            if (Reason != null)
            {
                Reason = $"Checking query string parameter with name: '{Name}' failed, because {Reason}";
            }
        }
    }
}
