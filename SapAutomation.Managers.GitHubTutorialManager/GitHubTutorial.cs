namespace SapAutomation.Managers.GitHubTutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using System.Collections.Generic;

    [MetaType("Tutorial")]
    public class GitHubTutorial : BaseNamedMetaObject
    {
        [MetaTypeValue("Tutorial folder")]
        public string Folder { get; set; }

        [MetaTypeCollection("Tutorial item list")]
        public List<GitHubTutorialItem> GitHubTutorialItems { get; set; }

        [MetaTypeValue("Path to generated tutorial", IsRequired = false)]
        public string PathToGeneratedTutorial { get; set; } = null;

        public Dictionary<string, GitHubTutorialFile> TutorialFiles = null;
    }
}
