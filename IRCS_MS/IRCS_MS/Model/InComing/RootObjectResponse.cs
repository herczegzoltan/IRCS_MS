using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IRCS_MS.Model.InComing
{
    [XmlRoot("RootObjectResponse")]
    public class RootObjectResponse
    {
        [XmlElement("Answer")]
        public List<Answer> Answer { get; set; }
    }
}
