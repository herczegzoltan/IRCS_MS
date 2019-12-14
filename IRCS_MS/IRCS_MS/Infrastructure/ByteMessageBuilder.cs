using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure
{

    public static class ByteMessageBuilder
    {
        private static List<byte> listOfBytes = new List<byte>() { };

        private static byte[] _arrayByteOutgoing = new byte[5] { 0x00, 0x00, 0x00, 0x00,0x00  };

        private static string[] _arrayByteIncoming = new string[3] { "", "", ""};

        public static void SetByteArray(int index, byte inputByte)
        {
            _arrayByteOutgoing[index] = inputByte;
        }

        public static void SetByteArray(int index, string inputByte)
        {
            _arrayByteOutgoing[index] = ConvertStringToByte(inputByte);
        }

        public static void SetByteIncomingArray(int index, string inputByte)
        {
            _arrayByteIncoming[index] = inputByte;
        }

        public static void ResetByteIncomingArray()
        {
            Array.Clear(_arrayByteIncoming, 0, _arrayByteIncoming.Length);

        }

        public static string[] GetByteIncomingArray()
        {
            return _arrayByteIncoming;
        }

        public static byte[] GetByteArray()
        {
            return _arrayByteOutgoing;
        }

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
