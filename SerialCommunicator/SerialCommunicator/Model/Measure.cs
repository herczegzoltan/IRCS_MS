using System.Xml.Serialization;

namespace SerialCommunicator.Model
{
    public class Measure
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }
    }
}