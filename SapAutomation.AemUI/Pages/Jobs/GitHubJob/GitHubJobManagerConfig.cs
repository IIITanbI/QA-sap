namespace SapAutomation.AemUI.Pages.Jobs.GitHubJob
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("GitHub job manager config")]
    public class GitHubJobManagerConfig : BaseMetaObject
    {
        [MetaTypeObject("GitHub job page definition")]
        public WebElement GitHubJobPageDefinition;
    }
}
