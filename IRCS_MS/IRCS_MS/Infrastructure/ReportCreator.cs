using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;

namespace IRCS_MS.Infrastructure
{
    public class ReportCreator
    {
        public string FilePath { get; set; }

        public string FileName { get; set; }

        private List<string> HeaderRowInsertable = null;
        private List<List<string>> HeaderRowInsertable2 = null;


        //TODO Constructor DI
        ExcelPackage excel = null;
        public ReportCreator()
        {
            excel = new ExcelPackage();
            HeaderRowInsertable = new List<string>() { };
            HeaderRowInsertable2 = new List<List<string>>() { };
        }

        public void AddData(List<string> data, List<List<string>> data2)
        {
            HeaderRowInsertable = data;
            HeaderRowInsertable2 = data2;
        }

        public void CreateFile()
        {
            char[] apha = "ABCDEFGHIJKLMNOPQRSTUVWXY".ToCharArray();

            using (excel)
            {
                excel.Workbook.Worksheets.Add("Worksheet1");

                string headerRange = "A1:" + HeaderRowInsertable.Count;

                var worksheet = excel.Workbook.Worksheets["Worksheet1"];

                //string headerRange = "A1:" + Char.ConvertFromUtf32(HeaderRowInsertable[0].Length + 64) + "1";


                worksheet.Cells[headerRange].LoadFromCollection(HeaderRowInsertable);


                int i = 0;
                string headerRange2 = "";
                foreach (var item in HeaderRowInsertable2)
                {
                    headerRange2 = apha[i + 1].ToString() + 0 + ":" + item.Count;
                    if (i == item.Count)
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                    worksheet.Cells[headerRange2].LoadFromCollection(item);


                }

                FileInfo excelFile = new FileInfo(@""+ FilePath +"\\" + FileName + ".xlsx");//+ FilePath + FileName + ".xlsx"); //C:\Users\Herczeg Zoltán\Desktop\test.xlsx");
                excel.SaveAs(excelFile);
            }
        }

    }
}
