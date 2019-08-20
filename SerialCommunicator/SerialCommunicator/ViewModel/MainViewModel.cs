using SerialCommunicator.Model;
using System.Windows.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerialCommunicator.Infrastructure;
using System.IO.Ports;
using System.Windows;
using System.Threading;

namespace SerialCommunicator.ViewModel
{
    public class MainViewModel : NotifyViewModel
    {
        #region Variables
        private ICommand m_ConnectCommand;

        private ICommand m_DisConnectCommand;

        private List<string> m_AvailablePorts;

        private List<int> m_BaudRates;

        private string m_SelectedAvailablePort;

        private int m_SelectedBaudRate;

        public ICommand CmdConnect => m_ConnectCommand;

        public ICommand CmdDisConnect => m_DisConnectCommand;

        SerialPort COMPort = null;

        private string m_StateOfDevice;

        private bool m_ConnectButtonIsEnabled = true ;

        private bool m_DisConnectButtonIsEnabled = false;

        #endregion

        public MainViewModel()
        {
            m_ConnectCommand = new DelegateCommand(ConnectToDevice);
            m_DisConnectCommand = new DelegateCommand(DisConnect);
            AvailablePorts = SerialCommunicationSettings.ListOfSerialPorts();
            BaudRates = SerialCommunicationSettings.ListOfSerialBaudRates();
        }

        private void ConnectToDevice()
        {
            COMPort = new SerialPort(SelectedAvailablePort, SelectedBaudRate);
            try
            {
                COMPort.Open();
                CmdConnectIsEnabled = false;
                CmdDisConnectIsEnabled = true;
                ReadingSerialState();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ReadingSerialState()
        {
            StateOfDevice = "State:" + COMPort.IsOpen.ToString();


            //await Task.Run(() =>
     
            //    while (true)
            //    {
            //        Application.Current.Dispatcher.Invoke(() =>
            //        {
            //            try
            //            {
            //            }
            //            catch (Exception e)
            //            {
            //                MessageBox.Show(e.ToString());
            //            }
            //        });
            //        Task.Delay(1000);
            //    }
            //});
        }

        private  void DisConnect()
        {
            CmdConnectIsEnabled = true;
            CmdDisConnectIsEnabled = true;
            COMPort.Close();
        }

        #region Properties

        public string SelectedAvailablePort
        {

            get
            {
                return m_SelectedAvailablePort;
            }

            set
            {
                m_SelectedAvailablePort = value;
                OnPropertyChanged("AvailablePorts");
            }
        }

        public int SelectedBaudRate
        {
            get
            {
                return m_SelectedBaudRate;
            }

            set
            {
                m_SelectedBaudRate = value;
                OnPropertyChanged("BaudRates");
            }
        }

        public List<string> AvailablePorts
        {
            get
            {
                return m_AvailablePorts;
            }
            set
            {
                m_AvailablePorts = value;
                OnPropertyChanged("AvailablePorts");
            }
        }

        public List<int> BaudRates
        {
            get
            {
                return m_BaudRates;
            }
            set
            {
                m_BaudRates = value;
                OnPropertyChanged("BaudRates");
            }
        }

        public string StateOfDevice
        {
            get
            {
                return m_StateOfDevice;
            }
            set
            {
                m_StateOfDevice = value;
                OnPropertyChanged("StateOfDevice");

            }
        }

        public bool CmdConnectIsEnabled
        {
            get
            {
                return m_ConnectButtonIsEnabled; ;
            }
            set
            {
                m_ConnectButtonIsEnabled = value;
                OnPropertyChanged("CmdConnectIsEnabled");
            }
        }

        public bool CmdDisConnectIsEnabled
        {
            get
            {
                return m_DisConnectButtonIsEnabled; ;
            }
            set
            {
                m_DisConnectButtonIsEnabled = value;
                OnPropertyChanged("CmdDisConnectIsEnabled");
            }
        }
        #endregion
    }
}