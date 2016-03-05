namespace SapAutomation.Web.Components.Sap.FacetsComponent
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;
    using QA.AutomatedMagic;

    [MetaType("Setup for Tutorial Catalog")]

    public class FacetsComponentConfig : BaseNamedMetaObject
    {
        [MetaTypeCollection("Setup for Tutorial Catalog", "namespace", IsRequired = false)]
        public List<string> Namespaces { get; set; } = new List<string>();

        [MetaTypeValue("Is Hide Facets", IsRequired = false)]
        public bool HideFacets { get; set; }

        [MetaTypeValue("Type of Selection", IsRequired = false)]
        public TypeOfSelection TypeOfSelection { get; set; } = TypeOfSelection.CheckBoxes;
    }

    public enum TypeOfSelection
    {
        CheckBoxes,
        DropDownList,
        List,
        RadioButtons
    }
}
