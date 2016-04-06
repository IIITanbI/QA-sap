namespace SapAutomation.Web.Components.Aem.InsertNewComponentForm
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.Managers.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [CommandManager("Insert new component manager")]
    public class InsertNewComponentFormManager : BaseCommandManager
    {
        [MetaSource(nameof(InsertNewComponentForm) + @"\InsertNewComponentFormWebDefenition.xml")]
        public WebElement InsertNewComponentFormWebDefenition { get; set; }

        public override void Init()
        {
            InsertNewComponentFormWebDefenition.Init();
        }

        [Command("Add component to page")]
        public void AddComponent(WebDriverManager wdm, string componentName, ILogger log)
        {
            var insertNewComponentEditor = InsertNewComponentFormWebDefenition["InsertNewComponent"];

            wdm.Click(insertNewComponentEditor["OtherExpander"], log);
            wdm.Click(insertNewComponentEditor[$"{componentName}"], log);
            Thread.Sleep(1000);
            wdm.Click(insertNewComponentEditor["NewComponentOK"], log);

            wdm.WaitForPageLoaded(log);
            wdm.WaitForJQueryLoaded(log);
        }
    }
}
