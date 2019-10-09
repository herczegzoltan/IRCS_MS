using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;

namespace SerialCommunicator.Infrastructure
{
    public class ReportCreator
    {
        public string FilePath { get; set; }

        public string FileName { get; set; }

        private List<string[]> HeaderRowInsertable = null;

        //TODO Constructor DI
        ExcelPackage excel = null;
        public ReportCreator()
        {
            excel = new ExcelPackage();
            HeaderRowInsertable = new List<string[]>() { };

        }

        public void AddData(List<string[]> data)
        {
            HeaderRowInsertable = data;
        }

        public void CreateFile()
        {
            using (excel)
            {
                excel.Workbook.Worksheets.Add("Worksheet1");
                string headerRange = "A1:" + Char.ConvertFromUtf32(HeaderRowInsertable[0].Length + 64) + "1";
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                worksheet.Cells[headerRange].LoadFromArrays(HeaderRowInsertable);

                FileInfo excelFile = new FileInfo(@""+ FilePath +"\\" + FileName + ".xlsx");//+ FilePath + FileName + ".xlsx"); //C:\Users\Herczeg Zoltán\Desktop\test.xlsx");
                excel.SaveAs(excelFile);
            }
        }

    }
}
