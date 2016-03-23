namespace SapAutomation.Managers.GitHubTutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using System.Collections.Generic;

    [MetaType("Tutorial")]
    public class GitHubTutorial : BaseNamedMetaObject
    {
        [MetaTypeValue("Tutorial folder", IsRequired = false)]
        public string Folder { get; set; } = "tutorials";

        [MetaTypeCollection("Tutorial item list")]
        public List<GitHubTutorialTest> GitHubTutorialTests { get; set; }
        
        public string PathToGeneratedTutorial { get; set; } = null;
    }
}
