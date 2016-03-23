namespace SapAutomation.Managers.GitHubTutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using System.Collections.Generic;

    [MetaType("Tutorial config")]
    public class GitHubTutorialManagerConfig : BaseMetaObject
    {
        [MetaTypeValue("Path to temp folder")]
        public string TempFolderPath { get; set; }
    }
}
