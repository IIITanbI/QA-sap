namespace SapAutomation.AemUI.Components.Facets
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using QA.AutomatedMagic.MetaMagic;

    [MetaType("Setup for Tutorial Catalog")]

    public class FacetsSetupConfig
    {
        [MetaTypeCollection("Setup for Tutorial Catalog")]
        public List<string> Namespaces { get; set; }

        [MetaTypeValue("Is Hide Facets")]
        public bool HideFacets { get; set; }

        [MetaTypeValue("Type of Selection")]
        public TypeOfSelection TypeOfSelection { get; set; }
    }

    public enum TypeOfSelection
    {
        CheckBoxes,
        DropDownList,
        List,
        RadioButtons
    }

}
