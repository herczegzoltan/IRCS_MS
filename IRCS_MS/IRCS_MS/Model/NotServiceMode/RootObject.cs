using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IRCS_MS.Model
{
    [XmlRoot("RootObject")]
    public class RootObject
    {
        [XmlElement("Card")]
        public  List<Card> Card { get; set; }

        [XmlElement("DefaultValues")]
        public DefaultValues ValuesCommands { get; set;}
    }
}
