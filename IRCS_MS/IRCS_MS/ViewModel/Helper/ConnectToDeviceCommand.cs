using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IRCS_MS.ViewModel.Helper
{
    public class ConnectToDeviceCommand
    {

        public ServiceModeViewModel MV { get; set; }


        public ConnectToDeviceCommand(ServiceModeViewModel mv)
        {
            MV = mv;
        }

        public void dd()
        {
            MV.MyProp = "asd";
        }
    }
}
