namespace SapAutomation.Managers.AemTagManager
{
    using QA.AutomatedMagic.MetaMagic;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [MetaType("Tag")]
    public class AemTag : BaseMetaObject
    {
        [MetaTypeValue("Tag name", IsRequired = false)]
        public string Name { get; set; } = null;

        [MetaTypeValue("Tag title", IsRequired = false)]
        public string Title { get; set; } = null;

        [MetaTypeValue("Tag name", IsRequired = false)]
        public bool NeedToCreate { get; set; } = true;

        [MetaTypeValue("Tag description", IsRequired = false)]
        public string Description { get; set; } = null;

        [MetaTypeObject("Parent tag", IsRequired = false)]
        public AemTag Parent { get; set; } = null;

        [MetaTypeValue("Tag type", IsRequired = false)]
        public TagType Type { get; set; }

        [MetaTypeCollection("Child tags", IsRequired = false)]
        public List<AemTag> ChildTags { get; set; } = null;

        [MetaTypeCollection("Tag path", IsRequired = false)]
        public string Path { get; set; } = null;

        public string GetFullName()
        {
            if (Parent == null)
                return $"{Name}:";
            if(Parent.Type == TagType.Namespace)
                return $"{Parent.GetFullName()}{Name}";
            else return $"{Parent.GetFullName()}/{Name}";
        }

        public enum TagType
        {
            Namespace,
            Tag
        }
    }
}
