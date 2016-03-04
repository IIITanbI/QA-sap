namespace SapAutomation.AemUI.Components.FinderResults
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic;

    [MetaType("Setup for finder results element")]
    public class FinderResultsConfig : BaseMetaObject
    {
        [MetaTypeValue("Number of pages for displaying", IsRequired = false)]
        public string Pagination { get; set; } = null;

        [MetaTypeValue("Showing description", IsRequired = false)]
        public bool ShowDescription { get; set; }

        [MetaTypeValue("Path to default document icon", IsRequired = false)]
        public string DefaultDocumentIcon { get; set; } = null;

        [MetaTypeValue("Path to default page icon", IsRequired = false)]
        public string DefaultPageIcon { get; set; } = null;

        [MetaTypeValue("Path to default video icon", IsRequired = false)]
        public string DefaultVideotIcon { get; set; } = null;

        [MetaTypeValue("Type value for alphabetical sorting", IsRequired = false)]
        public AlphabeticalSorting AlphabeticalSorting { get; set; } = AlphabeticalSorting.Ascending;

        [MetaTypeValue("Type value for alphabetical sorting", IsRequired = false)]
        public SortingByDate SortingByDate { get; set; } = SortingByDate.Newest;
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
