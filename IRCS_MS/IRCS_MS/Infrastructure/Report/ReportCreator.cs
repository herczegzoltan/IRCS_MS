using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;
using IRCS_MS.Infrastructure.XmlHandler;

namespace IRCS_MS.Infrastructure
{
    public class ReportCreator
    {
        public string FilePath { get; set; }

        public string FileName { get; set; }

        private string Name { get; set; }

        private List<string> _measuredCardsColum = null;

        private List<string> HeaderRowInsertable = null;
        
        private List<string> HeaderZeroColumInsertable = null;
        
        private List<List<string>> HeaderRowInsertable2 = null;

        //TODO Constructor DI
        private ExcelPackage _excel = null;
        public ReportCreator()
        {
            _excel = new ExcelPackage();
            HeaderRowInsertable = new List<string>() { };
            HeaderRowInsertable2 = new List<List<string>>() { };
            HeaderZeroColumInsertable = new List<string>() { };
            _measuredCardsColum = new List<string>() { };
        }

        public void AddData(List<string> data, List<List<string>> data2, string name, List<string> measuredCardsColum)
        {
            HeaderRowInsertable = data;
            HeaderRowInsertable2 = data2;
            Name = name;
            _measuredCardsColum = measuredCardsColum;
        }

        private void FixExtraValues()
        {
            HeaderZeroColumInsertable.Insert(0,"IRCS_Measurement System");
            HeaderZeroColumInsertable.AddRange(_measuredCardsColum);

            HeaderRowInsertable.Insert(0, " ");
            HeaderRowInsertable.Insert(1, "Auto Measure:");
            HeaderRowInsertable.Insert(2, "Name:" + Name);
                       
            foreach (var item in HeaderRowInsertable2)
            {
                item.Insert(0, "");
                item.Insert(2, "");
            }
        }

        public void CreateFile()
        {
            FixExtraValues();

            char[] apha = "BCDEFGHIJKLMNOPQRSTUVWXY".ToCharArray();

            using (_excel)
            {
                _excel.Workbook.Worksheets.Add("Worksheet1");

                string headerRange0 = "A1:" + HeaderRowInsertable.Count;
                string headerRange = "B1:" + HeaderRowInsertable.Count;

                var worksheet = _excel.Workbook.Worksheets["Worksheet1"];

                //string headerRange = "A1:" + Char.ConvertFromUtf32(HeaderRowInsertable[0].Length + 64) + "1";
                worksheet.Cells[headerRange0].LoadFromCollection(HeaderZeroColumInsertable);
                worksheet.Cells[headerRange].LoadFromCollection(HeaderRowInsertable);

                int i = 0;
                string headerRange2 = "";
                foreach (var item in HeaderRowInsertable2)
                {
                    headerRange2 = apha[i + 1].ToString() + 0 + ":" + item.Count;
                    if(i == item.Count)
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                    worksheet.Cells[headerRange2].LoadFromCollection(item);
                }

                LoopThrough(worksheet);
                OtherCellsModification(worksheet);

                FileInfo excelFile = new FileInfo(@""+ FilePath +"\\" + FileName + ".xlsx");    //+ FilePath + FileName + ".xlsx"); //C:\Users\Herczeg Zoltán\Desktop\test.xlsx");
                _excel.SaveAs(excelFile);
            }
        }

        private void ColorChanger(int row, int colum, ExcelWorksheet excelWorksheet, Color cellBackGroundColor)
        {
            using (var range = excelWorksheet.Cells[row, colum])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(cellBackGroundColor);
            }
        }

        private void LoopThrough(ExcelWorksheet workSheet)
        {
            var start = workSheet.Dimension.Start;
            var end = workSheet.Dimension.End;
            for (int row = start.Row; row <= end.Row; row++)
            {
                for (int col = start.Column; col <= end.Column; col++)
                {
                    object cellValue = workSheet.Cells[row, col].Text;

                    //Here check the current cell value and change cell's color

                    if (XmlFilter.Instance.ContainTheRespone(cellValue.ToString()) && cellValue.ToString() == "Measure_OK")
                    {
                        ColorChanger(row, col, workSheet, Color.LightGreen);
                    }
                    else if (XmlFilter.Instance.ContainTheRespone(cellValue.ToString()) && cellValue.ToString() != "Measure_OK")
                    {
                        ColorChanger(row, col, workSheet, Color.Red);
                    }
                    else
                    {
                        //Nothing to do yet.
                    }
                }
            }
        }

        private void OtherCellsModification(ExcelWorksheet workSheet)
        {
            workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns].Merge = true;
            workSheet.Cells[1, 1, 1, workSheet.Dimension.Columns].Style.Border.Bottom.Style = ExcelBorderStyle.Double;

            workSheet.Cells[2, 1, workSheet.Dimension.Rows, workSheet.Dimension.Columns].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[2, 1, workSheet.Dimension.Rows, workSheet.Dimension.Columns].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[2, 1, workSheet.Dimension.Rows, workSheet.Dimension.Columns].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            workSheet.Cells[2, 1, workSheet.Dimension.Rows, workSheet.Dimension.Columns].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            workSheet.Cells[1, workSheet.Dimension.Columns, workSheet.Dimension.Rows, workSheet.Dimension.Columns].Style.Border.Right.Style = ExcelBorderStyle.Thick;

            workSheet.Cells[workSheet.Dimension.Rows, 1, workSheet.Dimension.Rows, workSheet.Dimension.Columns].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;

            workSheet.Cells[3, 2, 3, workSheet.Dimension.Columns].Merge = true;
            workSheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[3, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //Rotation
            var headerCells = workSheet.Cells[1, 3, 2, workSheet.Dimension.Columns];//sorok és oszlopok mettől meddig
            headerCells.Style.TextRotation = 90;


            workSheet.Cells.AutoFitColumns();
            workSheet.Row(2).Height = 100;
        }
    }
}
