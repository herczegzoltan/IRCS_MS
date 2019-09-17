using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using SerialCommunicator.Model;

namespace SerialCommunicator.Resource
{
    static public class XmlProcessor
    {
        public static RootObject GetXmlContent()
        {

            XmlSerializer serializer = new XmlSerializer(typeof(RootObject));

            Assembly assembly = Assembly.GetExecutingAssembly();

            const string xmlRead = "SerialCommunicator.Resource.functionsTable.xml";

            using (Stream stream = assembly.GetManifestResourceStream(xmlRead))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    RootObject result = (RootObject)serializer.Deserialize(reader);

                    return result ;
                }
            }
        }
    }
}
