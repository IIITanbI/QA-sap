namespace SapAutomation.Web.Components.Sap.FinderResultsComponent
{
    using OpenQA.Selenium;
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    [CommandManager("Finder results manager")]
    public class FinderResultsComponentManager : BaseCommandManager
    {
        [MetaSource(nameof(FinderResultsComponent) + @"/FinderResultsComponentWebDefinition.xml")]
        public WebElement FinderResultsComponentWebDefinition { get; set; }

        [Command("Command for setup edit finder results")]
        public void SetupFinderResults(WebDriverManager webDriverManager, FinderResultsComponentConfig finderResultsComponentConfig, ILogger log)
        {
            var finderResultsEditorElement = FinderResultsComponentWebDefinition["FinderResultsEditor"];

            webDriverManager.Click(FinderResultsComponentWebDefinition["EditButtonForFinderResults"], log);
            if (!string.IsNullOrEmpty(finderResultsComponentConfig.Pagination))
            {
                webDriverManager.Click(finderResultsEditorElement["NumberOfResultsArrow"], log);
                var tmp = FinderResultsComponentWebDefinition["NumberOfResultsItem"];
                var tmp1 = MetaType.CopyObjectWithCast(tmp);
                tmp1.ParentElement = tmp.ParentElement;
                tmp1.Locator.XPath = tmp1.Locator.XPath.Replace("toReplace", finderResultsComponentConfig.Pagination.ToString());
                webDriverManager.Click(tmp1, log);
            }

            if (!string.IsNullOrEmpty(finderResultsComponentConfig.DefaultDocumentIcon))
            {
                webDriverManager.SendChars(finderResultsEditorElement["DefaultDocumentIcon"], finderResultsComponentConfig.DefaultDocumentIcon, log);
            }
            if (!string.IsNullOrEmpty(finderResultsComponentConfig.DefaultPageIcon))
            {
                webDriverManager.SendChars(finderResultsEditorElement["DefaultPageIcon"], finderResultsComponentConfig.DefaultPageIcon, log);
            }
            if (!string.IsNullOrEmpty(finderResultsComponentConfig.DefaultVideotIcon))
            {
                webDriverManager.SendChars(finderResultsEditorElement["DefaultVideoIcon"], finderResultsComponentConfig.DefaultVideotIcon, log);
            }

            webDriverManager.Click(finderResultsEditorElement["SortingConfigurationTab"], log);

            switch (finderResultsComponentConfig.AlphabeticalSorting)
            {
                case AlphabeticalSorting.Ascending:
                    webDriverManager.Click(finderResultsEditorElement["AscendingAlphabetSorting"], log);
                    break;
                case AlphabeticalSorting.Descending:
                    webDriverManager.Click(finderResultsEditorElement["DescendingAlphabetSorting"], log);
                    break;
                case AlphabeticalSorting.Both:
                    webDriverManager.Click(finderResultsEditorElement["BothAlphabetSorting"], log);
                    break;
                case AlphabeticalSorting.None:
                    webDriverManager.Click(finderResultsEditorElement["NoneAlphabetSorting"], log);
                    break;
            }

            switch (finderResultsComponentConfig.SortingByDate)
            {
                case SortingByDate.Newest:
                    webDriverManager.Click((finderResultsEditorElement["NewestSortinfByDate"]), log);
                    break;
                case SortingByDate.Oldest:
                    webDriverManager.Click((finderResultsEditorElement["OldestSortinfByDate"]), log);
                    break;
                case SortingByDate.Both:
                    webDriverManager.Click((finderResultsEditorElement["BothSortinfByDate"]), log);
                    break;
                case SortingByDate.None:
                    webDriverManager.Click((finderResultsEditorElement["NoneSortingByDate"]), log);
                    break;
            }

            webDriverManager.Click(finderResultsEditorElement["ButtonOK"], log);

            Thread.Sleep(5000);
        }
    }
}
