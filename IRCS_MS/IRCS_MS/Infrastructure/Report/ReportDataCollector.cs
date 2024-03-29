﻿using IRCS_MS.Infrastructure.XmlHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure
{
    public static class ReportDataCollector
    {
        private static List<List<string>> HorizontalList = null;

        private static List<string> VerticalList = null;


        public static void InitializeLists()
        {
            HorizontalList = new List<List<string>>() { };

            VerticalList = new List<string>() { };
        }

        public static void AddToVertical(string inputStr)
        {
            VerticalList.Add(inputStr);
        }

        public static void AddToVerticalAtIndex(int index, string inputStr)
        {
            VerticalList.Insert(index, inputStr);
        }

        public static void AddVerticalToHorizontal()
        {
            HorizontalList.Add(VerticalList);
        }

        public static void CleanerVertical()
        {
            VerticalList = new List<string>() { };
        }

        public static List<List<string>> GetTotal()
        {
            return HorizontalList;
        }
        
        public static void ClearAll()
        {
            CleanerVertical();
            HorizontalList = new List<List<string>>() { };
        }

        public static List<string> FillColumnForReport(bool IsCommonInclided, string selectedCardType)
        {
            List<string> temp = new List<string>() { };

            if (IsCommonInclided)
            {
                temp.Add("");
                temp.Add("");
                temp.Add(XmlFilter.Instance.GetDefaultName());
                temp.Add("");
                temp.Add("");
                temp.Add(selectedCardType);

                return temp;
            }
            else
            {
                temp.Add("");
                temp.Add("");
                temp.Add(selectedCardType);
                return temp;
            }
        }
    }
}
