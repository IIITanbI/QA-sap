namespace SapAutomation.Managers.AemUserManager
{
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("User manager config")]
    public class UserManagerConfig : BaseMetaObject
    {
        [MetaTypeValue("Path to user")]
        public string UserPath { get; set; }

        [MetaTypeValue("Path to user group")]
        public string GroupPath { get; set; }
    }
}
