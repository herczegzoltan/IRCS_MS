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

            GetEOF();
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

        public string GetSelectedCardTypeValue(string cardType)
        {
            string singleCardTypeValue = _rootOject.Card.Where(x => x.Name == cardType)
                                        .Select(n => n.Value).First();

            return singleCardTypeValue;
            //byte temp = ByteMessageBuilder.ConvertStringToByte(singleCardTypeValue);

            //ByteMessageBuilder.SetByteArray(1,temp);
        }

        public string GetSelectedMeasurementValue(string cardType, string measurement)
        {

            string singleMeasureValue = _rootOject.Card.Where(x => x.Name == cardType)
                                       .Select(n => n.Measure).First()
                                       .Where(l => l.Name == measurement)
                                       .Select(k => k.Value).First();

            return singleMeasureValue;

            // byte temp = ByteMessageBuilder.ConvertStringToByte(singleMeasureValue);

            //    ByteMessageBuilder.SetByteArray(2, temp);
        }

        public void GetClosingByte()
        {
            //string singleCloseCharacter = _rootOject.
        }

        public string GetEOF()
        {
            string singleEof = _rootOject.ValuesCommands.Record.Where(x => x.Name == "EOF")
                .Select(n => n.Value).First();
            return singleEof;
        }

        public string GetMeasureOn()
        {
            string singleMeasureOn = _rootOject.ValuesCommands.Record.Where(x => x.Name == "MeasureOn")
                .Select(n => n.Value).First();
            return singleMeasureOn;
        }


        public string GetMeasureOff()
        {
            string singleMeasureOff = _rootOject.ValuesCommands.Record.Where(x => x.Name == "MeasureOff")
                .Select(n => n.Value).First();
            return singleMeasureOff;
        }


        public string GetConnect()
        {
            string singleConnect = _rootOject.ValuesCommands.Record.Where(x => x.Name == "Connect")
                .Select(n => n.Value).First();
            return singleConnect;
        }

        public string GetRun()
        {
            string singleRun = _rootOject.ValuesCommands.Record.Where(x => x.Name == "Run")
                .Select(n => n.Value).First();
            return singleRun;
        }

    }
}
