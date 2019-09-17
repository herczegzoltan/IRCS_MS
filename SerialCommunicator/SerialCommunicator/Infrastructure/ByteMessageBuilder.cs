using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunicator.Infrastructure
{
    public static class ByteMessageBuilder
    {
        private static List<byte> listOfBytes = new List<byte>() { };

        public static void AddNewToByteList(byte newByte)
        {
            listOfBytes.Add(newByte);
        }

        public static void ClearByteList()
        {
            listOfBytes.Clear();
        }

        public static List<byte> GetByteList()
        {
            return listOfBytes;
        }

        public static byte ConvertStringToByte(string strByte)
        {
            int value = Convert.ToInt32(strByte, 16);
            byte byteVal = Convert.ToByte(value);

            return byteVal;
        }
    }
}
