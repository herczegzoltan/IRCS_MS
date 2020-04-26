using IRCS_MS.Model.Common;
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
        public List<Channel> Channel { get; set; }

        [XmlElement("DefaultValues")]
        public DefaultValues DefaultValues { get; set; }
    }
}
