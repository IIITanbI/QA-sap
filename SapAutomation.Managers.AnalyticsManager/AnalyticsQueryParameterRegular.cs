namespace SapAutomation.Managers.AnalyticsManager
{
    using System;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("AnalyticsQueryParameterRegular")]
    public class AnalyticsQueryParameterRegular : AnalyticsQueryParameter
    {
        [MetaTypeValue("Analytics query parameter value", IsRequired = false)]
        public string ExpectedValue { get; set; } = null;

        public override void Check()
        {
            if (ActualValue != null && ExpectedValue != null)
            {
                if (!ExpectedValue.Equals(ActualValue))
                {
                    Reason = $"Actual value: '{ActualValue}' isn't equal expected value: '{ExpectedValue}'.";
                }
                else return;
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
