using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using IRCS_MS.Model;
using IRCS_MS.Model.InComing;
using IRCS_MS.Model.NotServiceMode;
using IRCS_MS.Model.SeviceMode;

namespace IRCS_MS.Infrastructure.XmlHandler
{
    public static class XmlProcessor
    {
        public static RootObject GetXmlRootObjectCommands()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RootObject));

            Assembly assembly = Assembly.GetExecutingAssembly();

            const string xmlRead = "IRCS_MS.Resource.commandsTable.xml";

            using (Stream stream = assembly.GetManifestResourceStream(xmlRead))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    RootObject result = (RootObject)serializer.Deserialize(reader);

                    return result ;
                }
            }
        }

        public static RootObjectResponse GetXmlRootObjectResponse()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RootObjectResponse));

            Assembly assembly = Assembly.GetExecutingAssembly();

            const string xmlRead = "IRCS_MS.Resource.responsesTable.xml";

            using (Stream stream = assembly.GetManifestResourceStream(xmlRead))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    RootObjectResponse result = (RootObjectResponse)serializer.Deserialize(reader);

                    return result;
                }
            }
        }

        public static ServiceRootObject GetXmlServiceRootObjectCommands()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ServiceRootObject));

            Assembly assembly = Assembly.GetExecutingAssembly();

            const string xmlRead = "IRCS_MS.Resource.serviceCommandsTable.xml";

            using (Stream stream = assembly.GetManifestResourceStream(xmlRead))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    ServiceRootObject result = (ServiceRootObject)serializer.Deserialize(reader);

                    return result;
                }
            }
        }

    }
}
