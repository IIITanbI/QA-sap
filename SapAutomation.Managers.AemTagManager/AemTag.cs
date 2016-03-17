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
        public string Name { get; set; }

        [MetaTypeValue("Tag title", IsRequired = false)]
        public string Title { get; set; }

        [MetaTypeValue("Tag description", IsRequired = false)]
        public string Description { get; set; } = null;

        [MetaTypeValue("Tag type", IsRequired = false)]
        public AemTagType TagType { get; set; }

        [MetaTypeCollection("Child tags", IsRequired = false)]
        public List<AemTag> ChildTags { get; set; } = null;

        [MetaTypeValue("Tag path", IsRequired = false)]
        public string Path { get; set; } = null;

        public AemTag Parent { get; set; } = null;

        public AemTag()
        {
            TagType = AemTagType.Tag;
        }

        public override void MetaInit()
        {
            if (ChildTags != null)
            {
                ChildTags.ForEach(c => c.Parent = this);
            }
        }

        public string GetFullName()
        {
            if (Parent == null)
                return $"{Name}:";
            if (Parent.TagType == AemTagType.Namespace)
                return $"{Parent.GetFullName()}{Name}";
            else return $"{Parent.GetFullName()}/{Name}";
        }
    }
}
