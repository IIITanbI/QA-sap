namespace SapAutomation.Web.Components.Sap.FinderResultsComponent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic;

    [MetaType("Setup for finder results element")]
    public class FinderResultsComponentConfig : BaseNamedMetaObject
    {
        //ResultsConfigurationTab

        [MetaTypeValue("Number of pages for displaying", IsRequired = false)]
        public string Pagination { get; set; } = null;

        [MetaTypeValue("Path to default document icon", IsRequired = false)]
        public string DefaultDocumentIcon { get; set; } = null;

        [MetaTypeValue("Path to default page icon", IsRequired = false)]
        public string DefaultPageIcon { get; set; } = null;

        [MetaTypeValue("Path to default video icon", IsRequired = false)]
        public string DefaultVideotIcon { get; set; } = null;

        //SortingConfigurationTab

        [MetaTypeValue("Value for alphabetical sorting", IsRequired = false)]
        public AlphabeticalSorting AlphabeticalSorting { get; set; } = AlphabeticalSorting.NotSpecified;

        [MetaTypeValue("Value for alphabetical sorting", IsRequired = false)]
        public SortingByDate SortingByDate { get; set; } = SortingByDate.NotSpecified;

        [MetaTypeValue("Value for alphabetical sorting", IsRequired = false)]
        public DefaultSorting DefaultSorting { get; set; } = DefaultSorting.NotSpecified;

        //LayoutTab

        [MetaTypeValue("Value for Result View", IsRequired = false)]
        public ResultView ResultView { get; set; } = ResultView.NotSpecified;

        [MetaTypeValue("Is description shown?", IsRequired = false)]
        public bool ShowDescription { get; set; } = false;

        [MetaTypeValue("Are tags shown?", IsRequired = false)]
        public bool ShowTags { get; set; } = false;

        [MetaTypeValue("Is Update Date shown?", IsRequired = false)]
        public bool ShowUpdateDate { get; set; } = false;
    }

    public enum AlphabeticalSorting
    {
        NotSpecified,
        Ascending,
        Descending,
        Both,
        None
    }

    public enum SortingByDate
    {
        NotSpecified,
        Newest,
        Oldest,
        Both,
        None
    }

    public enum DefaultSorting
    {
        NotSpecified,
        Newest,
        Ascending
    }

    public enum ResultView
    {
        NotSpecified,
        List,
        Grid
    }
}
