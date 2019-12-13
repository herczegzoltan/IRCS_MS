using System.Xml.Serialization;

namespace IRCS_MS.Model
{
    public class Answer
    {
        [XmlAttribute("translate")]
        public string Translate { get; set; }

        [XmlAttribute("command")]
        public string Command { get; set; }

        [XmlAttribute("data")]
        public string Data { get; set; }

        [XmlAttribute("eof")]
        public string Eof { get; set; }
    }
}