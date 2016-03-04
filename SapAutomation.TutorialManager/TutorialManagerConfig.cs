namespace SapAutomation.TutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("Tutorial config")]
    public class TutorialManagerConfig : BaseMetaObject
    {
        [MetaTypeValue("Path to temp folder")]
        public string TempFolderPath { get; set; }
    }
}
