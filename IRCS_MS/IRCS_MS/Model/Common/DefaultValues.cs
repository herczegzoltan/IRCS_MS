using System.Collections.Generic;
using System.Xml.Serialization;

namespace IRCS_MS.Model.Common
{
    public class DefaultValues
    {
        [XmlElement("Record")]
        public List<Record> Record { get; set; }

    }
}