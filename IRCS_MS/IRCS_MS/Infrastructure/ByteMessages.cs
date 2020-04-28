using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure
{
    public class ByteMessages
    {
        public byte[] MeasureModeIncoming = new byte[3] { 0x00, 0x00, 0x00 };

        public byte[] MeasureModeOutgoing = new byte[5] { 0x00, 0x00, 0x00, 0x00, 0x00 };

        public byte[] ServiceModeIncoming = new byte[5] { 0x00, 0x00, 0x00, 0x00, 0x00 };

        public byte[] ServiceModeOutgoing = new byte[5] { 0x00, 0x00, 0x00, 0x00, 0x00 };

        private static ByteMessages instance;
        private ByteMessages()
        {
        }
        public static ByteMessages Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ByteMessages();
                }
                return instance;
            }
        }

    }
}
