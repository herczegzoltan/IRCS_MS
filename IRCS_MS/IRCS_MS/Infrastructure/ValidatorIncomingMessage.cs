using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure
{

    public static class ValidatorIncomingMessage
    {
        public static bool CheckRightEOF(string incomingByte, XmlFilter xmlFilter)
        {
            incomingByte = ByteMessageBuilder.ConvertDecimalStringToHexString(incomingByte);

            if (xmlFilter.GetEOF() == incomingByte)
            {
                return true;
            }
            return false;
        }


        public static bool ErrorMessageBack(XmlFilter xmlData, string incomingData)
        {
            return !xmlData.GetValidator(incomingData);
        }
    }
}
