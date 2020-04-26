using System.Xml.Serialization;

namespace IRCS_MS.Model.Common
{
    public class Measure
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}