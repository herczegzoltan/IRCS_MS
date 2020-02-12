﻿using IRCS_MS.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Helper
{
    public static class GeneralMessageCollection
    {
        public static string GeneralMessageRecived(string customText)
        {
            return "Info: " + DateTime.Now.ToString("HH:mm:ss").ToString() + " -> " + customText + "\n";
        }
        public static int LoopCounter = 0;

        public static string GeneralMessageRecived(string customText, XmlFilter xmlData)
        {
            return "Info: " + DateTime.Now.ToString("HH:mm:ss").ToString() + " -> " +
                              xmlData.GetSelectedCardTypeName
                              (ByteMessageBuilder.ConvertDecimalStringToHexString(ByteMessageBuilder.GetByteIncomingArray()[0].ToString()))
                              + " -> " +
                              xmlData.GetCurrentMeasurement(xmlData.GetSelectedCardTypeName
                              (ByteMessageBuilder.ConvertDecimalStringToHexString(ByteMessageBuilder.GetByteIncomingArray()[0].ToString())), LoopCounter)
                              + " -> " +
                              xmlData.GetResponseData
                              (ByteMessageBuilder.ConvertDecimalStringToHexString(ByteMessageBuilder.GetByteIncomingArray()[1].ToString()))
                              + customText + "\n";
        }

        public static string LogIntoFile(Exception ex)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if(File.Exists(docPath + "\\IRCS_MS_LogFile.txt"))
            {
                File.AppendAllText(
                    docPath + "\\IRCS_MS_LogFile.txt", Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") +"\n" + ex.ToString() + Environment.NewLine);
            }
            else
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "IRCS_MS_LogFile.txt")))
                {
                    outputFile.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\n" + ex.ToString());
                }

            }
            //if it exists no need to create new 

            return "Error occurred! Logfile was saved to " + docPath.ToString();
        }
    }
}
