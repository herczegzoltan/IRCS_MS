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
    public static class XmlFilter
    {

        public static List<string> GetCardTypeNames(RootObject result)
        {
            IEnumerable<String> cardTypes = result.Card.Select(x => x.Name);

            return new List<string>(cardTypes);
        }

        public static string GetSelectedCardTypeValue(RootObject result, string cardType)
        {
            string singleCardTypeValue = result.Card.Where(x => x.Name == cardType)
                                        .Select(n => n.Value).First();

            return singleCardTypeValue;
        }

        public static List<string> GetMeasurements(RootObject result, string cardType)
        {
            IEnumerable<IEnumerable<String>> measure = result.Card.Where(x => x.Name == cardType)
                                         .Select(m => m.Measure.Select(l => l.Name));

            IEnumerable<String> measureList = measure.SelectMany(s => s);

            return new List<string>(measureList);
        }

        public static string GetSelectedMeasurementValue(RootObject result, string cardType, string measurement)
        {

            string singleMeasureValue = result.Card.Where(x => x.Name == cardType)
                                       .Select(n => n.Measure).First()
                                       .Where(l => l.Name == measurement)
                                       .Select(k => k.Value).First();

            return singleMeasureValue;
        }
    }
}
