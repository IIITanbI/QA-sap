namespace SapAutomation.Web.Pages.Sap.ContactUsPage
{
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("Contact us page manager config")]
    public class ContactUsPageManagerConfig : BaseNamedMetaObject
    {
        [MetaTypeValue("WritingAbout", IsRequired = false)]
        public WritingAbouts WritingAbout { get; set; } = WritingAbouts.NONE;

        [MetaTypeValue("Industry", IsRequired = false)]
        public Industries Industry { get; set; } = Industries.INDA000001;

        [MetaTypeValue("SAPSolutionOfInterest", IsRequired = false)]
        public SAPSolutionsOfInterest SAPSolutionOfInterest { get; set; } = SAPSolutionsOfInterest.NONE;

        [MetaTypeValue("CompanyRevenue", IsRequired = false)]
        public CompanyRevenues CompanyRevenue { get; set; } = CompanyRevenues.NONE;

        [MetaTypeValue("Message", IsRequired = false)]
        public string Message { get; set; } = null;

        [MetaTypeValue("OfficeLocation", IsRequired = false)]
        public OfficeLocations OfficeLocation { get; set; } = OfficeLocations.NONE;

        [MetaTypeValue("Salutation", IsRequired = false)]
        public Salutations Salutation { get; set; } = Salutations.Mr;

        [MetaTypeValue("ContactMethod", IsRequired = false)]
        public ContactMethods ContactMethod { get; set; } = ContactMethods.NONE;

        [MetaTypeValue("FirstName", IsRequired = false)]
        public string FirstName { get; set; } = null;

        [MetaTypeValue("Company", IsRequired = false)]
        public string Company { get; set; } = null;

        [MetaTypeValue("LastName", IsRequired = false)]
        public string LastName { get; set; } = null;

        [MetaTypeValue("RelationshipToSap", IsRequired = false)]
        public RelationshipsToSap RelationshipToSap { get; set; } = RelationshipsToSap.NONE;

        [MetaTypeValue("LastName", IsRequired = false)]
        public string Email { get; set; } = null;

        [MetaTypeValue("City", IsRequired = false)]
        public string City { get; set; } = null;

        [MetaTypeValue("Street", IsRequired = false)]
        public string Street { get; set; } = null;

        [MetaTypeValue("PostalCode", IsRequired = false)]
        public string PostalCode { get; set; } = null;

        [MetaTypeValue("Phone", IsRequired = false)]
        public string Phone { get; set; } = null;
    }

    public enum WritingAbouts
    {
        NONE,
        Analyst,
        Careers,
        EduTrain,
        EventsCommunities,
        General,
        Industry,
        InformationWorkers,
        Invest,
        SiteIssues,
        OnDemandSolutions,
        Partner,
        Platforms,
        Press,
        BusinessObjects,
        UserGroups,
        Services,
        Solutions,
        Sponsorships,
        Upgrade
    }

    public enum Industries
    {
        NONE,
        INDA000001,
        INDA000002,
        INDA000003,
        INDA000004,
        INDA000005,
        INDA000006,
        INDA000007,
        INDA000009,
        INDA000010,
        INDA000011,
        INDA000013,
        INDA000014,
        INDA000015,
        INDA000017,
        INDA000018,
        INDA000019,
        INDA000020,
        INDA000022,
        INDA000023,
        INDA000025,
        INDA000037,
        INDA000026,
        INDA000029,
        INDA000027,
        INDA000028
    }

    public enum SAPSolutionsOfInterest
    {
        NONE,
        SOLA000038,
        SOLA000052,
        SOLA000040,
        SOLA000021,
        SOLA000043,
        SOLA000047,
        SOLA000036,
        SOLA000012,
        SOLA000075,
        SOLA000064,
        SOLA000026,
        SOLA000032,
        SOLB004008,
        SOLA000062,
        SOLA000030,
        SOLA000083,
        SOLA000029,
        SOLB017003,
        SOLA000045,
        SOLA000084,
        SOLA000014,
        SOLA000080,
        SOLA000085,
        SOLA000020,
        SOLA000074,
        SOLB018002,
        SOLB018001,
        SOLA000028,
        SOLB004007,
        SOLB004009,
        SOLA000008,
        SOLA000046,
        SOLA000067,
        SOLA000003,
        SOLA000005,
        SOLA000004,
        SOLA000006,
        SOLA000069,
        SOLA000072,
        SOLA000070,
        SOLA000053,
        SOLA000068,
        SOLA000055,
        SOLA000056,
        SOLA000082,
        SOLA000081,
        SOLA000017,
        SOLA000009,
        SOLA000027,
        SOLA000065,
        SOLA000094,
        SOLA000061,
        SOLA000011,
        SOLA000018,
        SOLA000042,
        SOLA000010,
        SOLA000086,
        SOLA000087,
        SOLA000063,
        SOLA000088,
        SOLA000089,
        SOLA000090,
        SOLA000091,
        SOLA000019,
        SOLA000016,
        SOLA000025,
        SOLA000001,
        SOLA000015,
        SOLA000050,
        SOLA000092,
        SOLA000095,
        SOLA000049,
    }

    public enum CompanyRevenues
    {
        NONE,
        COMPREV01,
        COMPREV02,
        COMPREV03,
        COMPREV04
    }

    public enum Salutations
    {
        NONE,
        Ms = 1,
        Mr = 2
    }

    public enum ContactMethods
    {
        NONE,
        email,
        mail,
        phone
    }

    public enum RelationshipsToSap
    {
        NONE,
        REL2,
        REL3,
        REL4,
        REL5,
        REL6,
        REL7,
        REL8,
        REL9,
        REL10
    }
}
