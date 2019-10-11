using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunicator.Infrastructure
{
    public static class ReportDataHelper
    {
        private static ReportCreator report = null;

        public static void InitializeMeasure(string _fileName, string _filePath)
        {
            report = new ReportCreator();
            report.FileName = _fileName;
            report.FilePath = _filePath;
        }



        public static List<List<string>> CreateDataMap(Measurement mt)
        {

            List<List<string>> snAndMo = new List<List<string>>() { };


            snAndMo.Add(mt.SchauerNumber.Concat(mt.ResultOfMeasurement).ToList());

            //List<string[]> rawdata = new List<string[]>() { };
            return snAndMo;


            //mt.SchauerNumber.Insert(0, " ");

            //rawdata.Add(mt.SchauerNumber.ToArray());

            //mt.MeasureType.RemoveAt(0);

            //for (int i = 0; i < mt.MeasureType.Count; i++)
            //{

            //    rawdata.Add(new string[] { mt.MeasureType.ElementAt(i), mt.ResultOfMeasurement.ElementAt(i) });

            //}
           // return rawdata;
        }

        public static void PassListTOReport(List<string> input, List<List<string>> input2)
        {
            report.AddData(input,input2);
        }

        public static void CreateReportFile()
        {
            report.CreateFile();
        }
    }
}
