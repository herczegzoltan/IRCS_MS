using IRCS_MS.Model;
using IRCS_MS.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IRCS_MS.Infrastructure
{
    public sealed class XmlFilter
    {
        private RootObject _rootOject;

        private RootObjectResponse _rootOjectResponse;

        public readonly int DefaultNumbersOfBytes = 3;

        public XmlFilter()
        {
            _rootOject = XmlProcessor.GetXmlRootObjectCommands();

            _rootOjectResponse = XmlProcessor.GetXmlRootObjectResponse();
            
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
            IEnumerable<IEnumerable<String>> measure = _rootOject.Card.Where(x => string.Equals(x.Name,cardType,StringComparison.OrdinalIgnoreCase))
                                         .Select(m => m.Measure.Select(l => l.Name));

            IEnumerable<String> measureList = measure.SelectMany(s => s);

            return new List<string>(measureList);
        }

        public List<string> GetMeasurementsWithoutAutoMeasure(string cardType)
        {

            IEnumerable<IEnumerable<String>> measure = _rootOject.Card.Where(x => string.Equals(x.Name, cardType, StringComparison.OrdinalIgnoreCase))
                                         .Select(m => m.Measure.Where(o => o.Name != "AutoMeasure").Select(l => l.Name));

            IEnumerable<String> measureList = measure.SelectMany(s => s);

            return new List<string>(measureList);
        }

        public string GetSelectedCardTypeValue(string cardType)
        {
            string singleCardTypeValue = _rootOject.Card.Where(x => x.Name == cardType)
                                        .Select(n => n.Value).First();

            return singleCardTypeValue;
           
        }

        public string GetSelectedCardTypeName(string cardValue)
        {
            string singleCardTypeName = _rootOject.Card.Where(x => x.Value == cardValue)
                                        .Select(n => n.Name).First();

            return singleCardTypeName;

        }

        public string GetSelectedMeasurementValue(string cardType, string measurement)
        {

            string singleMeasureValue = _rootOject.Card.Where(x => x.Name == cardType)
                                       .Select(n => n.Measure).First()
                                       .Where(l => l.Name == measurement)
                                       .Select(k => k.Value).First();

            return singleMeasureValue;
        }

        public int GetNumberOfExpectedMeasureState(string cardType)
        {
            int counter = _rootOject.Card.Where(c => string.Equals(c.Name, cardType, StringComparison.OrdinalIgnoreCase))
                          .Select(n => n.Measure).First()
                          .Where(l => l.Name != "AutoMeasure").Count();

            return counter;
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

        public string GetServiceOn()
        {
            string singleServiceOn = _rootOject.ValuesCommands.Record.Where(x => x.Name == "ServiceOn")
                .Select(n => n.Value).First();
            return singleServiceOn;
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

        public string GetDisConnect()
        {
            string singleDisConnect = _rootOject.ValuesCommands.Record.Where(x => x.Name == "DisConnect")
                .Select(n => n.Value).First();
            return singleDisConnect;
        }

        public string GetRun()
        {
            string singleRun = _rootOject.ValuesCommands.Record.Where(x => x.Name == "Run")
                .Select(n => n.Value).First();
            return singleRun;
        }

        public string GetResponseTranslate(string command, string data, string eof)
        {
            command = ByteMessageBuilder.ConvertDecimalStringToHexString(command);
            data = ByteMessageBuilder.ConvertDecimalStringToHexString(data);
            eof = ByteMessageBuilder.ConvertDecimalStringToHexString(eof);
            //string.Equals(x.Command, command, StringComparison.OrdinalIgnoreCase)
            string singleResponseTranslate = _rootOjectResponse.Answer.Where(x => (string.Equals(x.Data, data, StringComparison.OrdinalIgnoreCase)) 
                                             && (string.Equals(x.Command, command, StringComparison.OrdinalIgnoreCase))
                                             && (string.Equals(x.Eof, eof, StringComparison.OrdinalIgnoreCase)))
               .Select(n => n.Translate).FirstOrDefault();
            return singleResponseTranslate;
        }

        public string GetResponseData(string data)
        {
            string singleResponseTranslateByData = _rootOjectResponse.Answer.Where(x => string.Equals(x.Data, data, StringComparison.OrdinalIgnoreCase))
                                                   .Select(h => h.Translate).FirstOrDefault();

            return singleResponseTranslateByData;
        }

        public string GetResponseEOF(string data)
        {
            string singleResponseEOFByData = _rootOjectResponse.Answer.Where(x => string.Equals(x.Data, data, StringComparison.OrdinalIgnoreCase))
                                                   .Select(h => h.Eof).FirstOrDefault();

            return singleResponseEOFByData;
        }

        public bool ContainTheRespone(string input)
        {
            bool singleResponseTranslate = _rootOjectResponse.Answer.Any(item => string.Equals(item.Translate, input, StringComparison.OrdinalIgnoreCase));

            return singleResponseTranslate;
        }

        public bool IsCommonIncluded(string cardType)
        {
            string isCommonIncluded = _rootOject.Card.Where(x => string.Equals(x.Name, cardType, StringComparison.OrdinalIgnoreCase))
                .Select(n => n.IsCommonIncluded).First();
            return bool.Parse(isCommonIncluded);
        }

        public string GetDefaultName()
        {
            string defaultCardName = _rootOject.Card.Where(x => x.Default == "true")
                .Select(n => n.Name).First();

            return defaultCardName.ToLower();
        }

        public int GetDefaultTimeOutValue()
        {
            string defaultCardName = _rootOject.ValuesCommands.Record.Where(x => x.Name == "TimeOutDefault")
                .Select(n => n.Value).First();

            return int.Parse(defaultCardName);
        }
        
        public bool GetValidator(string data)
        {
            //string defaultValidator = _rootOjectResponse.Answer.Where(x => x.Translate == data && x.Validation == "true")
            //    .Select(f => f.Validation).FirstOrDefault();
            //return bool.Parse(defaultValidator);


            //bool asd = _rootOjectResponse.Answer.Any(x => x.Validation == "true");


            //command = ByteMessageBuilder.ConvertDecimalStringToHexString(command);
            //data = ByteMessageBuilder.ConvertDecimalStringToHexString(data);
            //eof = ByteMessageBuilder.ConvertDecimalStringToHexString(eof);
            ////string.Equals(x.Command, command, StringComparison.OrdinalIgnoreCase)
            //bool singleResponse = _rootOjectResponse.Answer.Any(x => (string.Equals(x.Data, data, StringComparison.OrdinalIgnoreCase))
            //                                 && (string.Equals(x.Command, command, StringComparison.OrdinalIgnoreCase))
            //                                 && (string.Equals(x.Eof, eof, StringComparison.OrdinalIgnoreCase))
            //                                 && (string.Equals(x.Validation, "true", StringComparison.OrdinalIgnoreCase)));
            //return singleResponse;


            //string defaultCardName = _rootOject.Card.Where(x => x.Default == "true")
            //  .Select(n => n.Name).First();

            //return defaultCardName.ToLower();

            //command = ByteMessageBuilder.ConvertDecimalStringToHexString(command);
            data = ByteMessageBuilder.ConvertDecimalStringToHexString(data);
            //eof = ByteMessageBuilder.ConvertDecimalStringToHexString(eof);
            //string.Equals(x.Command, command, StringComparison.OrdinalIgnoreCase)
            bool singleResponseTranslate = _rootOjectResponse.Answer.Any(x => (string.Equals(x.Data, data, StringComparison.OrdinalIgnoreCase))
                                             && (string.Equals(x.Validation, "true", StringComparison.OrdinalIgnoreCase)));

            
            return singleResponseTranslate;
        }
    }
}
