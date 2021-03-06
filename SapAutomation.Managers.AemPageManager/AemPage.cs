﻿namespace SapAutomation.Managers.AemPageManager
{
    using QA.AutomatedMagic;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("AemPage")]
    public class AemPage : BaseNamedMetaObject
    {
        [MetaTypeValue("Aem page title", IsRequired = false)]
        public string Title { get; set; } = null;

        [MetaTypeValue("Parent aem page path", IsRequired = false)]
        public string ParentPath { get; set; } = null;

        [MetaTypeValue("Parent aem page path using in Prod", IsRequired = false)]
        public string ProdParentPath { get; set; } = null;

        [MetaTypeValue("Aem page template", IsRequired = false)]
        public string Template { get; set; } = null;

        [MetaTypeValue("Aem page path", IsRequired = false)]
        public string Path { get; set; } = null;

        [MetaTypeValue("Aem page status", IsRequired = false)]
        public string Status { get; set; } = null;
    }
}
