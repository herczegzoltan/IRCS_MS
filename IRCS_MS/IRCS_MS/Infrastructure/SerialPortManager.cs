using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRCS_MS.Infrastructure
{
    public sealed class SerialPortManager : SerialPort
    {
        //Private Constructor.  
        private SerialPortManager()
        {
        }
        private static readonly object padlock = new object();
        private static SerialPortManager instance = null;
        public static SerialPortManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SerialPortManager();
                    }
                    return instance;
                }
            }
        }

    }
}
