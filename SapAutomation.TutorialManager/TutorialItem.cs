namespace SapAutomation.TutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using System.Collections.Generic;

    [MetaType("Tutorial item")]
    public class TutorialItem : BaseMetaObject
    {
        [MetaTypeValue("Tutorial item folder name")]
        public string FolderName { get; set; }

        [MetaTypeCollection("Tutorial item files")]
        public List<TutorialFile> TutorialFiles { get; set; }
    }
}
