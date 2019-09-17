using System.Collections.Generic;
using System.Xml.Serialization;

namespace SerialCommunicator.Model
{
    public class Card
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        [XmlElement("Measure")]
        public List<Measure> Measure { get; set; }
    }
}