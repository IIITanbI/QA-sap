namespace SapAutomation.Managers.GitHubTutorialManager
{
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    [MetaType("GitHub Issue")]
    public class GitHubIssue
    {
        [MetaTypeValue("Content")]
        public string Content { get; set; }
    }
}
