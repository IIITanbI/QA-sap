namespace SapAutomation.Web.Pages.Sap.NewsletterPage
{
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("Newsletter page manager config")]
    public class NewsletterPageConfig : BaseNamedMetaObject
    {
        [MetaTypeValue("Office location country code", IsRequired = false)]
        public string OfficeLocation { get; set; } = null;

        [MetaTypeValue("Email", IsRequired = false)]
        public string Email { get; set; } = null;
    }
}
