using System.Collections.Generic;
using System.Xml.Serialization;

namespace IRCS_MS.Model
{
    public class Card
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlElement("Measure")]
        public List<Measure> Measure { get; set; }

        [XmlAttribute("iscommon")]
        public string IsCommonIncluded { get; set; }

        [XmlAttribute("default")]
        public string Default{ get; set; }



    }
}