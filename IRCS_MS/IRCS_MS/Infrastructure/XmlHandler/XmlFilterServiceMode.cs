using IRCS_MS.Infrastructure.Message;
using IRCS_MS.Model;
using IRCS_MS.Model.InComing;
using IRCS_MS.Model.MeasureMode;
using IRCS_MS.Model.ServiceMode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IRCS_MS.Infrastructure.XmlHandler
{
    public sealed class XmlFilterServiceMode
    {
        private ServiceRootObject _rootServiceObject;

        public readonly int DefaultNumbersOfBytes = 3;

        public XmlFilterServiceMode()
        {
            _rootServiceObject = XmlProcessor.GetXmlServiceRootObjectCommands();

            GetEOF();
        }

        private static XmlFilterServiceMode instance = null;

        public static XmlFilterServiceMode Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new XmlFilterServiceMode();
                }
                return instance;
            }
        }

        public string GetEOF()
        {
            string singleEof = _rootServiceObject.DefaultValues.Record.Where(x => x.Name == "EOF")
                .Select(n => n.Value).First();
            return singleEof;
        }


        #region Service Mode Filters

        public List<string> GetChannelNames()
        {
            IEnumerable<String> channelTypes = _rootServiceObject.Channel.Select(x => x.Name);

            return new List<string>(channelTypes);
        }

        public List<string> GetSubChannelNames(string channelType)
        {
            IEnumerable<IEnumerable<String>> subCahnnel = _rootServiceObject.Channel.Where(x => string.Equals(x.Name, channelType, StringComparison.OrdinalIgnoreCase))
                                         .Select(m => m.Measure.Select(l => l.Name));

            IEnumerable<String> subChannelList = subCahnnel.SelectMany(s => s);

            return new List<string>(subChannelList);
        }

        public List<string> GetDefaultValuesByTag(string input)
        {
            IEnumerable<String> result = _rootServiceObject.DefaultValues.Record.Where(x => string.Equals(x.Tag, input, StringComparison.OrdinalIgnoreCase))
                             .Select(m => m.Name);

            return new List<string>(result);
        }

        #endregion
    }
}