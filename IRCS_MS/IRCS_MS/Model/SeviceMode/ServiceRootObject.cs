using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IRCS_MS.Model.SeviceMode
{
    [XmlRoot("RootObject")]
    public class ServiceRootObject
    {
        [XmlElement("Channel")]
        public List<Channel> Card { get; set; }

        [XmlElement("DefaultValues")]
        public DefaultValues ValuesCommands { get; set; }
    }
}
