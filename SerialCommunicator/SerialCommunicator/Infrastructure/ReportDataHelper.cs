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


        public static void SetDataForReport(Measurement mt)
        {
            report.AddData(mt);
        }

        public static void CreateReportFile()
        {
            report.CreateFile();
        }
     
    }
}
