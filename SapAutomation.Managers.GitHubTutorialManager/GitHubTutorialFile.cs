namespace SapAutomation.Managers.GitHubTutorialManager
{
    using AemTagManager;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using System.Collections.Generic;
    using Web.Pages.Sap.TutorialCatalogPage;

    [MetaType("Tutorial file")]
    public class GitHubTutorialFile : BaseMetaObject
    {
        [MetaTypeValue("Tutorial file name")]
        public string Name { get; set; }

        [MetaTypeValue("Tutorial title", IsRequired = false)]
        public string Title { get; set; } = null;

        [MetaTypeValue("Tutorial description", IsRequired = false)]
        public string Description { get; set; } = null;

        [MetaTypeCollection("Tutorial tags", IsRequired = false, IsAssignableTypesAllowed = true)]
        public List<AemTag> Tags { get; set; } = null;

        [MetaTypeValue("Tutorial content", IsRequired = false)]
        public string Content { get; set; } = null;

        [MetaTypeCollection("Tutorial issue", IsRequired = false)]
        public List<GitHubTutorialIssue> Issues { get; set; } = new List<GitHubTutorialIssue>();

        public List<TutorialCard> Cards { get; set; } = new List<TutorialCard>();
    }
}
