namespace SapAutomation.Managers.AnalyticsManager
{
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("AnalyticsObject")]
    public class AnalyticsObject : BaseNamedMetaObject
    {
        [MetaTypeValue("Analytics object name")]
        public string Name { get; set; }

        [MetaTypeValue("Analytics object value", IsRequired = false)]
        public string ExpectedValue { get; set; } = null;

        public string ActualValue { get; set; } = null;

        public string Reason { get; set; } = null;

        public void Check()
        {
            if (ActualValue != null && ExpectedValue != null)
            {
                if (!ExpectedValue.Equals(ActualValue))
                {
                    Reason = $"Actual value: '{ActualValue}' isn't equal expected value: '{ExpectedValue}'";
                }
                else return;
            }
            else if (ActualValue == null && ExpectedValue == null)
            {
                return;
            }
            else if (ActualValue == null)
            {
                Reason = $"Actual value is missing, but expected value is '{ExpectedValue}'";
            }
            else
            {
                Reason = $"Expected value isn't specified, but actual value is '{ActualValue}'";
            }
            if (Reason != null)
            {
                Reason = $"Checking query string parameter with name: '{Name}' failed. {Reason}";
            }
        }
    }
}
