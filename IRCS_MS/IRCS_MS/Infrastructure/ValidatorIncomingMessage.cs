using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure
{
    public static class ValidatorIncomingMessage
    {
        private static bool CheckRightEOF(string incomingByte, XmlFilter xmlFilter)
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

        public static bool ValidationEOF(XmlFilter xmlData)
        {
            bool val = CheckRightEOF(ByteMessageBuilder.GetByteIncomingArray()[2].ToString(), xmlData);
            //validate EOF
            //if (!val)
            //{
            //    //MessageBox.Show("Uart Error!");
            //}
            return val;
        }
    }
}
