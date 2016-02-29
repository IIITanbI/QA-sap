namespace SapAutomation.TutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using System.Collections.Generic;

    [MetaType("Tutorial file")]
    public class TutorialFile : BaseMetaObject
    {
        [MetaTypeValue("Tutorial file name")]
        public string Name { get; set; }

        [MetaTypeValue("Tutorial title")]
        public string Title { get; set; }

        [MetaTypeValue("Tutorial description")]
        public string Description { get; set; }

        [MetaTypeCollection("Tutorial tags")]
        public List<string> Tags { get; set; }

        [MetaTypeValue("Tutorial content")]
        public string Content { get; set; }
    }
}
