namespace SapAutomation.Managers.GitHubTutorialManager
{
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Web.Pages.Sap.TutorialCatalogPage;

    [MetaType("GitHub Tutorial Test")]
    public class GitHubTutorialTest : BaseNamedMetaObject
    {
        [MetaTypeValue("Name")]
        public string Name { get; set; }

        [MetaTypeValue("Description")]
        public string Description { get; set; }

        [MetaTypeObject("GitHub Tutorial File")]
        public GitHubTutorialFile TutorialFile { get; set; }

        [MetaTypeObject("Expected issue", IsRequired = false)]
        public GitHubTutorialIssue ExpectedIssue { get; set; } = null;

        [MetaTypeObject("Expected card", IsRequired = false)]
        public TutorialCard ExpectedCard { get; set; } = null;

        public List<GitHubTutorialIssue> ActualIssues { get; set; } = new List<GitHubTutorialIssue>();

        public List<TutorialCard> ActualCardsOnAuthor { get; set; } = new List<TutorialCard>();
        public List<TutorialCard> ActualCardsOnPublish { get; set; } = new List<TutorialCard>();

        public override string ToString()
        {
            return $"Name : {Name}\n" +
                $"Description : {Description}\n" +
                $"TutorialFile : {TutorialFile}\n" +
                $"ExpectedIssue : {ExpectedIssue}\n" +
                $"ExpectedCard : {ExpectedCard}\n" +
                $"ActualIssues : {ActualIssues.Aggregate("", (s, a) => s+=a.ToString() + '\n')}\n" +
                $"ActualCardsOnAuthor : {ActualCardsOnAuthor.Aggregate("", (s, a) => s += a.ToString() + '\n')}\n" +
                $"ActualCardsOnPublish : {ActualCardsOnPublish.Aggregate("", (s, a) => s += a.ToString() + '\n')}\n";
        }
    }
}
