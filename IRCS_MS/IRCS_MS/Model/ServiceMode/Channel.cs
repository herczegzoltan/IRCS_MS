using System.Collections.Generic;
using System.Xml.Serialization;

namespace IRCS_MS.Model.ServiceMode
{
     public class Channel
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlElement("SubChannel")]
        public List<SubChannel> Measure { get; set; }

        [XmlAttribute("iscommon")]
        public string IsCommonIncluded { get; set; }

        [XmlAttribute("default")]
        public string Default { get; set; }

    }
}