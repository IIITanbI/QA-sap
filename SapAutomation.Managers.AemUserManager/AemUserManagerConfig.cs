namespace SapAutomation.Managers.AemUserManager
{
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("User manager config")]
    public class AemUserManagerConfig : BaseMetaObject
    {
        [MetaTypeObject("Admin user")]
        public AemUser Admin { get; set; }

        [MetaTypeValue("Path to user", IsRequired = false)]
        public string UserPath { get; set; }

        [MetaTypeValue("Path to user group", IsRequired = false)]
        public string GroupPath { get; set; }
    }
}
