namespace IRCS_MS.Model
{
    using System.Collections.Generic;

    public class Udp
    {
        public readonly string IPADDRESS = "192.168.1.122";
        public readonly int PORT = 23400;

        public bool IsEnabled { get; set; }
        public bool IsFinished { get; set; }

        public byte[] ReceivedBytes { get; set; }

        public List<byte> SendBytes { get; set; }

        public Udp()
        {
            IsEnabled = false;
            IsFinished = false;
            
            ReceivedBytes = new byte[3];
            SendBytes = new List<byte>();
        }
    }
}