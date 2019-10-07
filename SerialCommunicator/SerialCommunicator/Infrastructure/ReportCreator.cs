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
        public string FilePath = "IRCSTest";//IRCS_"CardName"_"kezdőszám"_"hány darab kártya lett mérve".

        public string FileName { get; set; }

        private List<string[]> HeaderRowInsertable = null;

        ExcelPackage excel = null;
        public ReportCreator()
        {
            excel = new ExcelPackage();
            HeaderRowInsertable = new List<string[]>() { };
        }

        public void AddNewRow(string[] NewRow)
        {
               HeaderRowInsertable.Add(NewRow);
        }

        public void CreateFile()
        {
            using (excel)
            {
                excel.Workbook.Worksheets.Add("Worksheet1");
                string headerRange = "A1:" + Char.ConvertFromUtf32(HeaderRowInsertable[0].Length + 64) + "1";
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];
                worksheet.Cells[headerRange].LoadFromArrays(HeaderRowInsertable);

                SaveToNewFile();

            }
        }


        public void SaveToNewFile()
        {
            FileInfo excelFile = new FileInfo(@""+FilePath +"\\" + "IRCSTest" + ".xlsx");//+ FilePath + FileName + ".xlsx"); //C:\Users\Herczeg Zoltán\Desktop\test.xlsx");
            excel.SaveAs(excelFile);
        }
    }
}
