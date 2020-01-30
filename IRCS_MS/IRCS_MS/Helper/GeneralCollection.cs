using IRCS_MS.Infrastructure;
using System;
using System.Collections.Generic;
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

        public static string GeneralMessageRecived(string customText, XmlFilter xmlData)
        {
            return "Info: " + DateTime.Now.ToString("HH:mm:ss").ToString() + " -> " +
                              xmlData.GetSelectedCardTypeName
                              (ByteMessageBuilder.ConvertDecimalStringToHexString(ByteMessageBuilder.GetByteIncomingArray()[0].ToString()))
                              + " -> " +
                              xmlData.GetResponseData
                              (ByteMessageBuilder.ConvertDecimalStringToHexString(ByteMessageBuilder.GetByteIncomingArray()[1].ToString()))
                              + customText + "\n";
        }
    }
}
