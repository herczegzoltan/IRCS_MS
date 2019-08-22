using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;


namespace SerialCommunicator.Infrastructure
{
    public static class SerialCommunicationSettings
    {
        public static List<string> ListOfSerialPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            return ports.ToList<string>();
        }

        public static List<int> ListOfSerialBaudRates()
        {
            return new List<int>() { 110, 150, 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600 };
        }
    }
}
