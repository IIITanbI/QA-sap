using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapAutomation.AemUI.Components.FinderResults
{
    public class FinderResultsConfig
    {
        public string Pagination { get; set; }
        public bool ShowDescription { get; set; }
        public string DefaultDocumentIcon { get; set; }
        public string DefaultPageIcon { get; set; }
        public string DefaultVideotIcon { get; set; }
        public AlphabeticalSorting AlphabeticalSorting { get; set; }
        public SortingByDate SortingByDate { get; set; }

        public FinderResultsConfig(string pagination, string defaultDocIcon, string defaultPageIcon, string defaultVideoIcon,
            bool showDescription = true, AlphabeticalSorting alphaSort = AlphabeticalSorting.Ascending, SortingByDate dateSort = SortingByDate.Newest)
        {
            Pagination = pagination;
            ShowDescription = showDescription;
            DefaultDocumentIcon = defaultDocIcon;
            DefaultPageIcon = defaultPageIcon;
            DefaultVideotIcon = defaultVideoIcon;
            AlphabeticalSorting = alphaSort;
            SortingByDate = dateSort;
        }
    }

    public enum AlphabeticalSorting
    {
        Ascending,
        Descending,
        Both,
        None
    }

    public enum SortingByDate
    {
        Newest,
        Oldest,
        Both,
        None
    }
}
