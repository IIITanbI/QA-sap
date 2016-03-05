namespace SapAutomation.Web.CustomComponents.FinderResultsComponent
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [CommandManager("Finder results manager")]
    public class FinderResultsComponentManager : BaseCommandManager
    {
        [MetaSource(nameof(FinderResultsComponent) + @"/FinderResultsComponentWebDefinition.xml")]
        public WebElement FinderResultsComponentWebDefinition { get; set; }

        [Command("Command for setup edit finder results", "SetUpFinderResults")]
        public void SetupFinderResults(WebDriverManager webDriverManager, FinderResultsComponentConfig finderResultsComponentConfig, ILogger log)
        {
            webDriverManager.Click(FinderResultsComponentWebDefinition["FinderResults.EditButtonForFinderResults"], log);
            if (!string.IsNullOrEmpty(finderResultsComponentConfig.Pagination))
            {
                webDriverManager.SendKeys(FinderResultsComponentWebDefinition["FinderResults.FinderResultsEditor.Pagination"], finderResultsComponentConfig.Pagination, log);
            }

            if (finderResultsComponentConfig.ShowDescription)
            {
                webDriverManager.CheckCheckbox(FinderResultsComponentWebDefinition["FinderResults.FinderResultsEditor.Description"], log);
            }
            else
            {
                webDriverManager.UnCheckCheckbox(FinderResultsComponentWebDefinition["FinderResults.FinderResultsEditor.Description"], log);
            }

            if (!string.IsNullOrEmpty(finderResultsComponentConfig.DefaultDocumentIcon))
            {
                webDriverManager.SendKeys(FinderResultsComponentWebDefinition["FinderResults.FinderResultsEditor.DefaultDocumentIcon"], finderResultsComponentConfig.DefaultDocumentIcon, log);
            }
            if (!string.IsNullOrEmpty(finderResultsComponentConfig.DefaultPageIcon))
            {
                webDriverManager.SendKeys(FinderResultsComponentWebDefinition["FinderResults.FinderResultsEditor.DefaultPageIcon"], finderResultsComponentConfig.DefaultPageIcon, log);
            }
            if (!string.IsNullOrEmpty(finderResultsComponentConfig.DefaultVideotIcon))
            {
                webDriverManager.SendKeys(FinderResultsComponentWebDefinition["FinderResults.FinderResultsEditor.DefaultVideoIcon"], finderResultsComponentConfig.DefaultVideotIcon, log);
            }

            webDriverManager.Click(FinderResultsComponentWebDefinition["FinderResults.FinderResultsEditor.SortingConfigurationTab"], log);

            switch (finderResultsComponentConfig.AlphabeticalSorting)
            {
                case AlphabeticalSorting.Ascending:
                    webDriverManager.Click(FinderResultsComponentWebDefinition["Root.FinderResultsEditor.AscendingAlphabetSorting"], log);
                    break;
                case AlphabeticalSorting.Descending:
                    webDriverManager.Click(FinderResultsComponentWebDefinition["Root.FinderResultsEditor.DescendingAlphabetSorting"], log);
                    break;
                case AlphabeticalSorting.Both:
                    webDriverManager.Click(FinderResultsComponentWebDefinition["Root.FinderResultsEditor.BothAlphabetSorting"], log);
                    break;
                case AlphabeticalSorting.None:
                    webDriverManager.Click(FinderResultsComponentWebDefinition["Root.FinderResultsEditor.NoneAlphabetSorting"], log);
                    break;
            }

            switch (finderResultsComponentConfig.SortingByDate)
            {
                case SortingByDate.Newest:
                    webDriverManager.Click((FinderResultsComponentWebDefinition["Root.FinderResultsEditor.NewestSortinfByDate"]), log);
                    break;
                case SortingByDate.Oldest:
                    webDriverManager.Click((FinderResultsComponentWebDefinition["Root.FinderResultsEditor.OldestSortinfByDate"]), log);
                    break;
                case SortingByDate.Both:
                    webDriverManager.Click((FinderResultsComponentWebDefinition["Root.FinderResultsEditor.BothSortinfByDate"]), log);
                    break;
                case SortingByDate.None:
                    webDriverManager.Click((FinderResultsComponentWebDefinition["Root.FinderResultsEditor.NoneSortingByDate"]), log);
                    break;
            }

            webDriverManager.Click(FinderResultsComponentWebDefinition["Root.FinderResultsEditor.ButtonOK"], log);

            webDriverManager.Refresh(log);
            webDriverManager.WaitForPageLoaded(log);
            webDriverManager.WaitForJQueryLoaded(log);
        }
    }
}
