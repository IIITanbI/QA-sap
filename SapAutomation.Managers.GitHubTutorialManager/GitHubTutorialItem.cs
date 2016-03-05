namespace SapAutomation.Managers.GitHubTutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using System.Collections.Generic;

    [MetaType("Tutorial item")]
    public class GitHubTutorialItem : BaseMetaObject
    {
        [MetaTypeValue("Tutorial item folder name")]
        public string FolderName { get; set; }

        [MetaTypeCollection("Tutorial item files")]
        public List<GitHubTutorialFile> TutorialFiles { get; set; }
    }
}
