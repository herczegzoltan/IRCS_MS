using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure
{
    public static class ByteMessageBuilderRepository
    {
        public static byte[] ClearArray(byte[] byteArray)
        {
            Array.Clear(byteArray, 0, byteArray.Length);

            return byteArray;
        }

        public static void ClearArray(string[] strArray)
        {
            Array.Clear(strArray, 0, strArray.Length);
        }

        public static void SetStrArrayByIndex(string[] byteArray, int index, string value)
        {
            byteArray[index] = value;

           // return byteArray;
        }



        public static byte[] SetByteArrayByIndex(byte[] byteArray, int index, string value)
        {
            byteArray[index] = ConverterRepository.ConvertStringToByte(value);

            return byteArray;
        }

        public static byte[] SetByteArrayByIndex(byte[] byteArray, int index, byte value)
        {
            byteArray[index] = value;

            return byteArray;
        }

        //public static byte ConvertStringToByte(string strByte)
        //{
        //    int value = Convert.ToInt32(strByte, 16);
        //    byte byteVal = Convert.ToByte(value);

        //    return byteVal;
        //}

        //public static string ConvertDecimalStringToHexString(string decimalNumber)
        //{
        //    int number = int.Parse(decimalNumber);
        //    string hex = "0x" + number.ToString("x");
        //    if (hex.Length == 3)
        //    {
        //        hex = hex.Insert(2, "0");

        //    }
        //    return hex;
        //}

    }
}
