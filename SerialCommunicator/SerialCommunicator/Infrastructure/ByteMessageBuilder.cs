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

        private static byte[] _arrayByteOutgoing = new byte[5] { 0x00, 0x00, 0x00, 0x00,0x00  };

        private static byte[] _arrayByteIncoming = new byte[3] { 0x00, 0x00, 0x00 };

        public static void SetByteArray(int index, byte inputByte)
        {

            _arrayByteOutgoing[index] = inputByte;
        }

        public static void SetByteArray(int index, string inputByte)
        {
            _arrayByteOutgoing[index] = ConvertStringToByte(inputByte);
        }

        public static byte[] GetByteArray()
        {
            return _arrayByteOutgoing;
        }

        //public static void AddNewToByteList(byte newByte)
        //{
        //    listOfBytes.Add(newByte);
        //}

        //public static void ClearByteList()
        //{
        //    listOfBytes.Clear();
        //}

        //public static List<byte> GetByteList()
        //{
        //    return listOfBytes;
        //}

        public static byte ConvertStringToByte(string strByte)
        {
            int value = Convert.ToInt32(strByte, 16);
            byte byteVal = Convert.ToByte(value);

            return byteVal;
        }
    }
}
