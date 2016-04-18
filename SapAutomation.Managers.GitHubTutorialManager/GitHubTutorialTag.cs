namespace SapAutomation.Managers.GitHubTutorialManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("GitHub Tutorial Tag")]
    public class GitHubTutorialTag
    {
        [MetaTypeValue("Tag")]
        public string Tag { get; set; }

        [MetaTypeValue("Is valid?", IsRequired = false)]
        public bool IsValid { get; set; } = true;

        public override string ToString()
        {
            return $"Tag: '{Tag}'\n" +
                $"Is valid: '{IsValid}'";
        }
    }
}
