using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure
{
    public static class ByteMessageConverterRepository
    {
        public static byte ConvertStringToByte(string strByte)
        {
            int value = Convert.ToInt32(strByte, 16);
            byte byteVal = Convert.ToByte(value);

            return byteVal;
        }

        public static string ConvertDecimalStringToHexString(string decimalNumber)
        {
            int number = int.Parse(decimalNumber);
            string hex = "0x" + number.ToString("x");
            if (hex.Length == 3)
            {
                hex = hex.Insert(2, "0");

            }
            return hex;
        }
    }
}
