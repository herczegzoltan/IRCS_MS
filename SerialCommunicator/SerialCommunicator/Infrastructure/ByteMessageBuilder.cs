using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunicator.Infrastructure
{
    public sealed class ByteMessageBuilder
    {
        public ByteMessageBuilder()
        {
        }

        private static ByteMessageBuilder instance = null;

        public static ByteMessageBuilder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ByteMessageBuilder();
                }
                return instance;
            }
        }


        private List<byte> listOfBytes = new List<byte>() { };

        public void AddNewToByteList(byte newByte)
        {
            listOfBytes.Add(newByte);
        }

        public void ClearByteList(byte newByte)
        {
            listOfBytes.Clear();
        }

        public List<byte> GetByteList()
        {
          
            return listOfBytes;
        }

    }
}
