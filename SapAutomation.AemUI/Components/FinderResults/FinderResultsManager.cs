namespace SapAutomation.AemUI.Components.FinderResults
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.WebDriverManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [CommandManager(typeof(FinderResultsManagerConfig), "Finder results manager")]
    public class FinderResultsManager : ICommandManager
    {
        public WebElement FinderResultComponent;

        public FinderResultsManager(FinderResultsManagerConfig config)
        {
            FinderResultComponent = config.FinderResultsComponent;
        }


        [Command("Command for setup edit finder results", "SetUpFinderResults")]
        public void SetupFinderResults(WebDriverManager wdm, string pagValue, string pathDocIcon,string pathPageIcon,string pathVideoIcon, ILogger log)
        {
            wdm.Click(FinderResultComponent["Root.EditButtonForFinderResults"], log);
            wdm.SendKeys(FinderResultComponent["Root.FinderResultsEditor.Pagination"], pagValue, log);
            wdm.Click(FinderResultComponent["Root.FinderResultsEditor.Description"], log);

            wdm.SendKeys(FinderResultComponent["Root.FinderResultsEditor.DefaultDocumentIcon"], pathDocIcon, log);
            wdm.SendKeys(FinderResultComponent["Root.FinderResultsEditor.DefaultPageIcon"], pathPageIcon, log);
            wdm.SendKeys(FinderResultComponent["Root.FinderResultsEditor.DefaultVideoIcon"], pathVideoIcon, log);

            wdm.Click(FinderResultComponent["Root.FinderResultsEditor.SortingConfigurationTab"], log);
            wdm.Click(FinderResultComponent["Root.FinderResultsEditor.DescendingAlphabetSorting"], log);
            wdm.Click((FinderResultComponent["Root.FinderResultsEditor.OldestSortinfByDate"]), log);

            wdm.Click(FinderResultComponent["Root.FinderResultsEditor.ButtonOK"], log);
        }

        [Command("Command for setup edit finder results", "SetUpFinderResults")]
        public void SetupFinderResults(WebDriverManager wdm, FinderResultsConfig config, ILogger log)
        {
            wdm.Click(FinderResultComponent["Root.EditButtonForFinderResults"], log);
            wdm.SendKeys(FinderResultComponent["Root.FinderResultsEditor.Pagination"], config.Pagination, log);
            wdm.Click(FinderResultComponent["Root.FinderResultsEditor.Description"], log);

            wdm.SendKeys(FinderResultComponent["Root.FinderResultsEditor.DefaultDocumentIcon"], config.DefaultDocumentIcon, log);
            wdm.SendKeys(FinderResultComponent["Root.FinderResultsEditor.DefaultPageIcon"], config.DefaultPageIcon, log);
            wdm.SendKeys(FinderResultComponent["Root.FinderResultsEditor.DefaultVideoIcon"], config.DefaultVideotIcon, log);

            wdm.Click(FinderResultComponent["Root.FinderResultsEditor.SortingConfigurationTab"], log);
            
            switch(config.AlphabeticalSorting)
            {
                case AlphabeticalSorting.Ascending:
                    wdm.Click(FinderResultComponent["Root.FinderResultsEditor.AscendingAlphabetSorting"], log);
                    break;
                case AlphabeticalSorting.Descending:
                    wdm.Click(FinderResultComponent["Root.FinderResultsEditor.DescendingAlphabetSorting"], log);
                    break;
                case AlphabeticalSorting.Both:
                    wdm.Click(FinderResultComponent["Root.FinderResultsEditor.BothAlphabetSorting"], log);
                    break;
                case AlphabeticalSorting.None:
                    wdm.Click(FinderResultComponent["Root.FinderResultsEditor.NoneAlphabetSorting"], log);
                    break;
            }

            switch(config.SortingByDate)
            {
                case SortingByDate.Newest:
                    wdm.Click((FinderResultComponent["Root.FinderResultsEditor.NewestSortinfByDate"]), log);
                    break;
                case SortingByDate.Oldest:
                    wdm.Click((FinderResultComponent["Root.FinderResultsEditor.OldestSortinfByDate"]), log);
                    break;
                case SortingByDate.Both:
                    wdm.Click((FinderResultComponent["Root.FinderResultsEditor.BothSortinfByDate"]), log);
                    break;
                case SortingByDate.None:
                    wdm.Click((FinderResultComponent["Root.FinderResultsEditor.NoneSortingByDate"]), log);
                    break;
            }

            wdm.Click(FinderResultComponent["Root.FinderResultsEditor.ButtonOK"], log);
        }
    }
}
