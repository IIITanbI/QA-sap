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

        [MetaTypeValue("Action applied on copying to git repository", IsRequired = false)]
        public GitHubTutorialAction TutorialAction { get; set; } = GitHubTutorialAction.Create;

        public string PathToGeneratedTutorial { get; set; } = null;

        public override string ToString()
        {
            return $"Folder: '{Folder}'\n" + 
                $"Action: '{TutorialAction.ToString()}'\n" + 
                $"Path to generated folder: '{PathToGeneratedTutorial}'";
        }
    }

    public enum GitHubTutorialAction
    {
        Create,
        Update
    }
}
