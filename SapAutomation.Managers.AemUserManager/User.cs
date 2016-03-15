namespace SapAutomation.Managers.AemUserManager
{
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("Aem user")]
    public class User : BaseMetaObject
    {
        [MetaTypeValue("Aem user login ID")]
        public string LoginID { get; set; }

        [MetaTypeValue("Aem user first name", IsRequired = false)]
        public string FirstName { get; set; } = null;

        [MetaTypeValue("Aem user last name")]
        public string LastName { get; set; }

        [MetaTypeValue("Aem user Mail", IsRequired = false)]
        public string Mail { get; set; } = null;

        [MetaTypeValue("Aem user password")]
        public string Password { get; set; }
    }
}
