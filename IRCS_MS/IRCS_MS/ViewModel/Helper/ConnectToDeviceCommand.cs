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
    public class ConnectToDeviceCommand : ICommand
    {

        public MainViewModel MV { get; set; }

        public event EventHandler CanExecuteChanged;

        public ConnectToDeviceCommand(MainViewModel mv)
        {
            MV = mv;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }


        //private void ConnectToDevice()
        //{

        // if (MV.SelectedAvailablePort == null)
        //    {
        //        MessageBox.Show("No selected COM Port!");
        //    }
        //    else if (MV.SelectedBaudRate == 0)
        //    {
        //        MessageBox.Show("No selected Baud Rate!");
        //    }
        //    else
        //    {
        //        COMPort = new SerialPort(SelectedAvailablePort, SelectedBaudRate);
        //        try
        //        {
        //            COMPort.Open();
        //            UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.ConnectAfterClick);
        //            _runningTask = true;
        //            ConfigureDevice();
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show(e.ToString());
        //        }
        //    }

        //}
    }
}
