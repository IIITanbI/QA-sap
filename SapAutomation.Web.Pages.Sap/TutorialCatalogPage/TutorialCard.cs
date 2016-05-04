namespace SapAutomation.Web.Pages.Sap.TutorialCatalogPage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;
    using System.Drawing;

    [MetaType("Tutorial card")]
    public class TutorialCard : BaseMetaObject
    {
        [MetaTypeValue("Tutorial card title", IsRequired = false)]
        public string Title { get; set; } = null;

        [MetaTypeValue("Tutorial card description", IsRequired = false)]
        public string Description { get; set; } = null;

        [MetaTypeCollection("Tutorial card tags", "tag", IsRequired = false)]
        public List<string> Tags { get; set; } = null;

        public Dictionary<string, string> TagLinks { get; set; } = null;

        public string Status { get; set; } = null;

        public Point Location { get; set; } = new Point(0, 0);

        public string Name { get; set; } = null;

        [MetaTypeValue("Card url", IsRequired = false)]
        public string URL { get; set; } = null;

        [MetaTypeValue("Content", IsRequired = false)]
        public string Content { get; set; } = null;

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Card name: '{Name ?? string.Empty}'");
            sb.AppendLine($"Card title: '{Title ?? string.Empty}'");
            sb.AppendLine($"Card description: '{Description ?? string.Empty}'");
            sb.AppendLine($"Card URL: '{URL ?? string.Empty}'");
            sb.AppendLine($"Card status: '{Status ?? string.Empty}'");
            sb.AppendLine($"Card content: '{Content ?? string.Empty}'");
            sb.AppendLine($"Card Tags count: '{Tags?.Count ?? 0}'");
            if (Tags != null)
            {
                sb.Append($"Card Tags: ");
                bool first = true;
                foreach (var tag in Tags)
                {
                    if (first)
                    {
                        first = false;
                        sb.Append(tag);
                    }
                    else
                    {
                        sb.Append($", {tag}");
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
