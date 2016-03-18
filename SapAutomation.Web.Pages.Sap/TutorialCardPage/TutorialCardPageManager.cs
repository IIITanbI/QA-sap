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

        [Command("Command for change tutorial card title")]
        public void ChangeTutorialCardTitle(WebDriverManager webDriverManager, string title, ILogger log)
        {
            var body = TutorialCardPageWebDefinition["TutorialCardPage_Body"];
            var editor = body["TutorialCardEditor_Form"];

            webDriverManager.Click(body["Edit_Button"], log);

            webDriverManager.SendKeys(editor["Selected_Tab.Text_Area.TutorialCardTitle_Row"], title, log);

            webDriverManager.Click(editor["OK_Button"], log);
        }
    }
}
