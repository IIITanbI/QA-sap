namespace SapAutomation.TutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using System.Collections.Generic;

    [MetaType("Tutorial")]
    public class Tutorial : BaseNamedMetaObject
    {
        [MetaTypeValue("Tutorial folder")]
        public string Folder { get; set; }

        [MetaTypeCollection("Tutorial item list")]
        public List<TutorialItem> TutorialItems { get; set; }
    }
}
