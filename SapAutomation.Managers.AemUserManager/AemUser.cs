namespace SapAutomation.Managers.AemUserManager
{
    using QA.AutomatedMagic.CommandsMagic;
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("Aem user")]
    public class AemUser : BaseNamedMetaObject
    {
        [MetaTypeValue("Aem user login ID")]
        public string Username { get; set; }

        [MetaTypeValue("Aem user password")]
        public string Password { get; set; }

        [MetaTypeValue("Aem user first name", IsRequired = false)]
        public string FirstName { get; set; } = null;

        [MetaTypeValue("Aem user last name", IsRequired = false)]
        public string LastName { get; set; } = null;

        [MetaTypeValue("Aem user Mail", IsRequired = false)]
        public string Mail { get; set; } = null;

        public CookieContainer Cookie { get; set; } = null;

        public void CheckAuthorization()
        {
            if (Cookie == null)
                throw new DevelopmentException($"User: {this} is not authorized");
        }

        public override string ToString()
        {
            return $"Username: {Username}\n" +
                $"Password: '{Password}'\n" + 
                $"Mail: '{Mail}'";
        }
    }
}
