namespace SapAutomation.Managers.AnalyticsManager
{
    using QA.AutomatedMagic.MetaMagic;
    using System.Text.RegularExpressions;

    [MetaType("AnalyticsQueryParameterWithFormat")]
    public class AnalyticsQueryParameterWithFormat : AnalyticsQueryParameter
    {
        [MetaTypeValue("Analytics query parameter value with format", IsRequired = false)]
        public string ExpectedValueFormat { get; set; } = null;

        public override void Check()
        {
            if (ActualValue != null && ExpectedValueFormat != null)
            {
                if (!Regex.IsMatch(ActualValue, ExpectedValueFormat))
                {
                    Reason = $"Actual value: '{ActualValue}' isn't match expected value format: '{ExpectedValueFormat}'.";
                }
                else return;
            }
            else if (ActualValue == null && ExpectedValueFormat == null)
            {
                return;
            }
            else if (ActualValue == null)
            {
                Reason = $"Actual value is missing, but expected value is '{ExpectedValueFormat}'.";
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
