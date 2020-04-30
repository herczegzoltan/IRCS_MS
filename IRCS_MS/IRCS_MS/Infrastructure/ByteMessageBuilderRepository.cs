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
    }
}
