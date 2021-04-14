using IRCS_MS.Infrastructure;
using IRCS_MS.Infrastructure.Message;
using IRCS_MS.Infrastructure.XmlHandler;
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
        private static string _getSelectCardTypeName = "";
        private static string _getCurrentMeasurement = "";
        private static string _getResponseData = "";

        public static string GeneralMessageRecived(string customText)
        {
            return "Info: " + DateTime.Now.ToString("HH:mm:ss").ToString() + " -> " + customText + "\n";
        }
        public static int LoopCounter = 0;

        public static string GeneralMessageRecivedTranslation(string customText)
        {
            if (ByteMessages.Instance.MeasureModeIncoming[0] != null 
                && ByteMessages.Instance.MeasureModeIncoming[1] != null 
                && ByteMessages.Instance.MeasureModeIncoming[2] != null)
            {
                _getSelectCardTypeName = XmlFilter.Instance.GetSelectedCardTypeName
                              (ConverterRepository.ConvertDecimalStringToHexString(ByteMessages.Instance.MeasureModeIncoming[0].ToString()));
                _getCurrentMeasurement = XmlFilter.Instance.GetCurrentMeasurement(XmlFilter.Instance.GetSelectedCardTypeName
                              (ConverterRepository.ConvertDecimalStringToHexString(ByteMessages.Instance.MeasureModeIncoming[0].ToString())), LoopCounter);
                _getResponseData = XmlFilter.Instance.GetResponseData
                              (ConverterRepository.ConvertDecimalStringToHexString(ByteMessages.Instance.MeasureModeIncoming[1].ToString()));
            }

            return "Info: " + DateTime.Now.ToString("HH:mm:ss").ToString() + " -> " +
                              _getSelectCardTypeName
                              + " -> " +
                              _getCurrentMeasurement
                              + " -> " +
                              _getResponseData
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

        public static string IsRunningStateChecker(bool state)
        {
            return state == true ? "Running..." : "Not Running...";
        }
    }
}
