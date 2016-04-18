namespace SapAutomation.Web.Components.Sap.FinderResultsComponent
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic.Managers.WebDriverManager;
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
        public void SetUpFinderResults(WebDriverManager webDriverManager, FinderResultsComponentConfig finderResultsComponentConfig, ILogger log)
        {
            log?.INFO($"Start to setup edit finder results");
            log?.USEFULL($"Setuping edit finder results component on page: '{webDriverManager.GetCurrentUrl()}'");

            try
            {
                webDriverManager.Click(FinderResultsComponentWebDefinition["EditFinderResults_Button"], log);

                var finderResultsEditor_Form = FinderResultsComponentWebDefinition["FinderResultsEditor_Form"];
                var selected_Tab = finderResultsEditor_Form["Selected_Tab"];

                //ResultsConfigurationTab

                webDriverManager.Click(finderResultsEditor_Form["ResultsConfiguration_Tab"], log);
                if (!string.IsNullOrEmpty(finderResultsComponentConfig.Pagination))
                {
                    webDriverManager.Click(selected_Tab["NumberOfResultsArrow"], log);
                    var tmp = FinderResultsComponentWebDefinition["NumberOfResults_Item"];
                    var tmp1 = MetaType.CopyObjectWithCast(tmp);
                    tmp1.ParentElement = tmp.ParentElement;
                    tmp1.Locator.XPath = tmp1.Locator.XPath.Replace("toReplace", finderResultsComponentConfig.Pagination.ToString());
                    webDriverManager.Click(tmp1, log);
                }
                if (finderResultsComponentConfig.DefaultDocumentIcon != null)
                    webDriverManager.SendChars(selected_Tab["DefaultDocumentIcon_Input"], finderResultsComponentConfig.DefaultDocumentIcon, log);
                if (finderResultsComponentConfig.DefaultPageIcon != null)
                    webDriverManager.SendChars(selected_Tab["DefaultPageIcon_Input"], finderResultsComponentConfig.DefaultPageIcon, log);
                if (finderResultsComponentConfig.DefaultVideotIcon != null)
                    webDriverManager.SendChars(selected_Tab["DefaultVideoIcon_Input"], finderResultsComponentConfig.DefaultVideotIcon, log);

                //SortingConfigurationTab

                webDriverManager.Click(finderResultsEditor_Form["SortingConfiguration_Tab"], log);
                switch (finderResultsComponentConfig.AlphabeticalSorting)
                {
                    case AlphabeticalSorting.NotSpecified:
                        break;
                    case AlphabeticalSorting.Ascending:
                        webDriverManager.Click(selected_Tab["AlphabetSorting_Ascending_RadioButton"], log);
                        break;
                    case AlphabeticalSorting.Descending:
                        webDriverManager.Click(selected_Tab["AlphabetSorting_Descending_RadioButton"], log);
                        break;
                    case AlphabeticalSorting.Both:
                        webDriverManager.Click(selected_Tab["AlphabetSorting_Both_RadioButton"], log);
                        break;
                    case AlphabeticalSorting.None:
                        webDriverManager.Click(selected_Tab["AlphabetSorting_None_RadioButton"], log);
                        break;
                }
                switch (finderResultsComponentConfig.SortingByDate)
                {
                    case SortingByDate.NotSpecified:
                        break;
                    case SortingByDate.Newest:
                        webDriverManager.Click(selected_Tab["SortingByDate_Newest_RadioButton"], log);
                        break;
                    case SortingByDate.Oldest:
                        webDriverManager.Click(selected_Tab["SortingByDate_Oldest_RadioButton"], log);
                        break;
                    case SortingByDate.Both:
                        webDriverManager.Click(selected_Tab["SortingByDate_Both_RadioButton"], log);
                        break;
                    case SortingByDate.None:
                        webDriverManager.Click(selected_Tab["SortingByDate_None_RadioButton"], log);
                        break;
                }
                switch (finderResultsComponentConfig.DefaultSorting)
                {
                    case DefaultSorting.NotSpecified:
                        break;
                    case DefaultSorting.Newest:
                        webDriverManager.Click(selected_Tab["DefaultSorting_Newest_RadioButton"], log);
                        break;
                    case DefaultSorting.Ascending:
                        webDriverManager.Click(selected_Tab["DefaultSorting_Ascending_RadioButton"], log);
                        break;
                    default:
                        break;
                }

                //LayoutTab

                webDriverManager.Click(finderResultsEditor_Form["Layout_Tab"], log);
                switch (finderResultsComponentConfig.ResultView)
                {
                    case ResultView.NotSpecified:
                        break;
                    case ResultView.List:
                        webDriverManager.Click(selected_Tab["ResultView_List_RadioButton"], log);
                        break;
                    case ResultView.Grid:
                        webDriverManager.Click(selected_Tab["ResultView_Grid_RadioButton"], log);
                        break;
                    default:
                        break;
                }
                if (finderResultsComponentConfig.ShowTags)
                    webDriverManager.CheckCheckbox(selected_Tab["Tags_Checkbox"], log);
                else
                    webDriverManager.UnCheckCheckbox(selected_Tab["Tags_Checkbox"], log);
                if (finderResultsComponentConfig.ShowUpdateDate)
                    webDriverManager.CheckCheckbox(selected_Tab["UpdateDate_Checkbox"], log);
                else
                    webDriverManager.UnCheckCheckbox(selected_Tab["UpdateDate_Checkbox"], log);
                if (finderResultsComponentConfig.ShowDescription)
                    webDriverManager.CheckCheckbox(selected_Tab["Description_Checkbox"], log);
                else
                    webDriverManager.UnCheckCheckbox(selected_Tab["Description_Checkbox"], log);


                webDriverManager.Click(finderResultsEditor_Form["OK_Button"], log);

                Thread.Sleep(5000);

                log?.INFO($"Setup edit finder results completed");
            }
            catch (Exception ex)
            {
                log?.ERROR("Error occurred during setuping edit finder results component");
                throw new DevelopmentException("Error occurred during setuping edit finder results component", ex,
                    $"Setuping edit finder results component on page: '{webDriverManager.GetCurrentUrl()}'");
            }
        }
    }
}
