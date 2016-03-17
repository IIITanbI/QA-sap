namespace SapAutomation.Managers.GitHubTutorialManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using System.Collections.Generic;
    using Web.Pages.Sap.TutorialCatalogPage;

    [MetaType("Tutorial file")]
    public class GitHubTutorialFile : BaseMetaObject
    {
        [MetaTypeValue("Tutorial file name")]
        public string Name { get; set; }

        [MetaTypeValue("Tutorial title")]
        public string Title { get; set; }

        [MetaTypeValue("Tutorial description")]
        public string Description { get; set; }

        [MetaTypeCollection("Tutorial tags", "tag")]
        public List<string> Tags { get; set; }

        [MetaTypeValue("Tutorial content")]
        public string Content { get; set; }

        [MetaTypeCollection("Tutorial issue", IsRequired = false)]
        public List<GitHubTutorialIssue> Issues { get; set; } = new List<GitHubTutorialIssue>();

        public List<TutorialCard> Cards { get; set; } = new List<TutorialCard>();
    }
}
