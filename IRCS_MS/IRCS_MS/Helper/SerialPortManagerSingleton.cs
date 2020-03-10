using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Helper
{
    public sealed class SerialPortManagerSingleton : SerialPort
    {


        private SerialPort _serialPort = null;

        private static SerialPortManagerSingleton instance = null;

        public static  SerialPortManagerSingleton serialPortManagerSingleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new SerialPortManagerSingleton();
                }
                return instance;
            }
        }

        public void SetUpConnection(string port, int baudrate)
        {
            _serialPort = new SerialPort(port, baudrate);
        }
    }
}
