using IRCS_MS.Infrastructure.XmlHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure.Message
{
    public static class ValidatorIncomingMessage
    {
        private static bool CheckRightEOF(string incomingByte)
        {
            incomingByte = ConverterRepository.ConvertDecimalStringToHexString(incomingByte);

            if (XmlFilter.Instance.GetEOF() == incomingByte)
            {
                return true;
            }
            return false;
        }

        public static bool ErrorMessageBack(string incomingData)
        {
            return !XmlFilter.Instance.GetValidator(incomingData);
        }

        public static bool ValidationEOF()
        {
            bool val = CheckRightEOF(ByteMessages.Instance.MeasureModeIncoming[2].ToString());
            //validate EOF
            //if (!val)
            //{
            //    //MessageBox.Show("Uart Error!");
            //}
            return val;
        }
    }
}
