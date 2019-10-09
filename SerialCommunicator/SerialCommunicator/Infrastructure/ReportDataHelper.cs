﻿using System;
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


        public static List<string[]> CreateDataMap(Measurement mt)
        {
            List<string[]> rawdata = new List<string[]>() { };

            mt.SchauerNumber.Insert(0, " ");

            rawdata.Add(mt.SchauerNumber.ToArray());

            List<string> temp = new List<string>() { };
            for (int i = 0; i < mt.MeasureType.Count; i++)
            {
                temp.Clear();
                for (int x = 0; x < i + 1; x++)
                {
                    if (x == i)
                    {
                        temp.Add(mt.ResultOfMeasurement[i]);
                    }
                    else
                    {
                        temp.Add(" ");
                    }
                }
                rawdata.Add(new string[] { mt.MeasureType.ElementAt(i) }.Concat(temp.ToArray()).ToArray());
            }

            return rawdata;
        }


        public static void PassListTOReport(List<string[]> input)
        {
            report.AddData(input);
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
