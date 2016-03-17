namespace SapAutomation.Web.Pages.Sap.TutorialCardPage
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TutorialCardPageManager
    {
        [MetaSource(nameof(TutorialCardPage) + @"/TutorialCardPageDefenition.xml")]
        public WebElement TutorialCardPageWebDefinition { get; set; }

        [Command("Command for setup edit finder results")]
        public void SetupFinderResults(WebDriverManager webDriverManager, ILogger log)
        {
            webDriverManager.Click(TutorialCardPageWebDefinition["EditButtonForTutorialCard"], log);

            var tutorialCardEditor = TutorialCardPageWebDefinition["TutorialCardEditor"];

            webDriverManager.SendKeys(tutorialCardEditor["TutorialCardTitle"], "Edited title", log);

            webDriverManager.SendKeys(tutorialCardEditor["TutorialCardDescription"], "Edited description", log);

            webDriverManager.Click(tutorialCardEditor["ButtonOK"], log);
        }
    }
}
