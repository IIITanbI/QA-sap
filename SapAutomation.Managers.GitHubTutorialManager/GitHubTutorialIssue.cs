namespace SapAutomation.Managers.GitHubTutorialManager
{
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    [MetaType("GitHubTutorial Issue")]
    public class GitHubTutorialIssue : BaseMetaObject
    {
        [MetaTypeValue("Title")]
        public string Title { get; set; }

        [MetaTypeValue("Content")]
        public string Content { get; set; }

        public override string ToString()
        {
            return $"Title: '{Title}'\n" +
                $"Content: '{Content}'";
        }
    }
}
