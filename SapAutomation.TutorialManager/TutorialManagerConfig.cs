namespace SapAutomation.TutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("Tutorial config")]
    public class TutorialManagerConfig : BaseMetaObject
    {
        [MetaTypeValue("Tutorial temp folder")]
        public string Folder { get; set; }
    }
}
