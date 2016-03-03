namespace SapAutomation.AemUI.Components.FinderResults
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("Setup for finder results element")]
    public class FinderResultsConfig
    {
        [MetaTypeValue("Number of pages for displaying", IsRequired = true)]
        public string Pagination { get; set; }

        [MetaTypeValue("Showing description")]
        public bool ShowDescription { get; set; }

        [MetaTypeValue("Path to default document icon")]
        public string DefaultDocumentIcon { get; set; }

        [MetaTypeValue("Path to default page icon")]
        public string DefaultPageIcon { get; set; }

        [MetaTypeValue("Path to default video icon")]
        public string DefaultVideotIcon { get; set; }

        [MetaTypeValue("Type value for alpabetical sorting")]
        public AlphabeticalSorting AlphabeticalSorting { get; set; }

        [MetaTypeValue("Type value for alpabetical sorting")]
        public SortingByDate SortingByDate { get; set; }

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
