using System.Collections.Generic;
using System.Xml.Serialization;

namespace SerialCommunicator.Model
{
    public class DefaultValues
    {
        [XmlElement("Record")]
        public List<Record> Record { get; set; }
    }
}