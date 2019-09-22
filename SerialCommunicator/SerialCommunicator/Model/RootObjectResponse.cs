using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SerialCommunicator.Model
{
    [XmlRoot("RootObjectResponse")]
    public class RootObjectResponse
    {
        [XmlElement("Answer")]
        public List<Answer> Answer { get; set; }
    }
}
