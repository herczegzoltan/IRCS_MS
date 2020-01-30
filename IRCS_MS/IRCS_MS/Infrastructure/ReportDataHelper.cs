using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure
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

        public static void PassListTOReport(List<string> input, List<List<string>> input2, string name, List<string> measurements)
        {
            report.AddData(input,input2,name, measurements);
        }

        public static void CreateReportFile()
        {
            report.CreateFile();
        }
    }
}
