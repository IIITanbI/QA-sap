namespace SapAutomation.AemPageManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("AemPage", keyName: nameof(Title))]
    public class AemPage : BaseMetaObject
    {
        [MetaTypeValue("Aem page title")]
        public string Title { get; set; } = null;

        [MetaTypeValue("Parent aem page path")]
        public string ParentPath { get; set; } = null;

        [MetaTypeValue("Aem page template")]
        public string Template { get; set; } = null;
    }
}
