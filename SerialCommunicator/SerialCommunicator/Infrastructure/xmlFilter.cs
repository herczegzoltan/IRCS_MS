using SerialCommunicator.Model;
using SerialCommunicator.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SerialCommunicator.Infrastructure
{
    public sealed class XmlFilter
    {
        private RootObject _rootOject;

        public XmlFilter()
        {
            _rootOject = XmlProcessor.GetXmlContent();
        }

        private static XmlFilter instance = null;

        public static XmlFilter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new XmlFilter();
                }
                return instance;
            }
        }

        public List<string> GetCardTypeNames()
        {
            IEnumerable<String> cardTypes = _rootOject.Card.Select(x => x.Name);

            return new List<string>(cardTypes);
        }

        public List<string> GetMeasurements(string cardType)
        {
            IEnumerable<IEnumerable<String>> measure = _rootOject.Card.Where(x => x.Name == cardType)
                                         .Select(m => m.Measure.Select(l => l.Name));

            IEnumerable<String> measureList = measure.SelectMany(s => s);

            return new List<string>(measureList);
        }

        public void GetSelectedCardTypeValue(string cardType)
        {
            string singleCardTypeValue = _rootOject.Card.Where(x => x.Name == cardType)
                                        .Select(n => n.Value).First();

            byte temp = ByteMessageBuilder.ConvertStringToByte(singleCardTypeValue);

            ByteMessageBuilder.AddNewToByteList(temp);

           // return singleCardTypeValue;
        }

        public void GetSelectedMeasurementValue(string cardType, string measurement)
        {

            string singleMeasureValue = _rootOject.Card.Where(x => x.Name == cardType)
                                       .Select(n => n.Measure).First()
                                       .Where(l => l.Name == measurement)
                                       .Select(k => k.Value).First();

            byte temp = ByteMessageBuilder.ConvertStringToByte(singleMeasureValue);

            ByteMessageBuilder.AddNewToByteList(temp);

            //return singleMeasureValue;
        }
    }
}
