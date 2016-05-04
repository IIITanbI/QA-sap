namespace SapAutomation.Web.Pages.Sap.ContactUsPage
{
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("Contact us page manager config")]
    public class ContactUsPageConfig : BaseNamedMetaObject
    {
        [MetaTypeValue("WritingAbout", IsRequired = false)]
        public string WritingAbout { get; set; } = null;

        [MetaTypeValue("Industry", IsRequired = false)]
        public string Industry { get; set; } = null;

        [MetaTypeValue("SAPSolutionOfInterest", IsRequired = false)]
        public string SAPSolutionOfInterest { get; set; } = null;

        [MetaTypeValue("CompanyRevenue", IsRequired = false)]
        public string CompanyRevenue { get; set; } = null;

        [MetaTypeValue("Message", IsRequired = false)]
        public string Message { get; set; } = null;

        [MetaTypeValue("OfficeLocation", IsRequired = false)]
        public string OfficeLocation { get; set; } = null;

        [MetaTypeValue("Salutation", IsRequired = false)]
        public string Salutation { get; set; } = null;

        [MetaTypeValue("ContactMethod", IsRequired = false)]
        public string ContactMethod { get; set; } = null;

        [MetaTypeValue("FirstName", IsRequired = false)]
        public string FirstName { get; set; } = null;

        [MetaTypeValue("Company", IsRequired = false)]
        public string Company { get; set; } = null;

        [MetaTypeValue("LastName", IsRequired = false)]
        public string LastName { get; set; } = null;

        [MetaTypeValue("RelationshipToSap", IsRequired = false)]
        public string RelationshipToSap { get; set; } = null;

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
}
