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

        [MetaTypeCollection("Tutorial tags", "tag", IsRequired = false)]
        public List<string> Tags { get; set; } = null;

        [MetaTypeValue("Tutorial content", IsRequired = false)]
        public string Content { get; set; } = null;

        [MetaTypeValue("Does file have issue?", IsRequired = false)]
        public bool HaveIssue { get; set; } = false;

        [MetaTypeValue("Should file be extracted as tutorial?", IsRequired = false)]
        public bool HaveCard { get; set; } = true;
    }
}
