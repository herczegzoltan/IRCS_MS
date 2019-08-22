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
        private ICommand _connectCommand;

        private ICommand _disConnectCommand;

        private ICommand _sendData;

        private List<string> _availablePorts;

        private List<int> _baudRates;

        private string _selectedAvailablePort = null;

        private string _messageSendText;

        private string _messageRecievedText;

        private int _selectedBaudRate = 0;

        public ICommand CmdConnect => _connectCommand;

        public ICommand CmdDisConnect => _disConnectCommand;

        public ICommand CmdSendData => _sendData;

        SerialPort COMPort = null;

        private string _stateOfDevice = "State: Not connected!";

        private bool _connectButtonIsEnabled = true;

        private bool _disConnectButtonIsEnabled = false;

        private bool _runningTask;

        #endregion

        public MainViewModel()
        {
            _connectCommand = new DelegateCommand(ConnectToDevice);
            _disConnectCommand = new DelegateCommand(DisConnect);
            _sendData = new DelegateCommand(SendData);
            AvailablePorts = SerialCommunicationSettings.ListOfSerialPorts();
            BaudRates = SerialCommunicationSettings.ListOfSerialBaudRates();
        }

        private void ConnectToDevice()
        {
            if (SelectedAvailablePort == null)
            {
                MessageBox.Show("No selected COM Port!");
            }
            else if (SelectedBaudRate == 0)
            {
                MessageBox.Show("No selected Baud Rate!");
            }
            else
            {
                COMPort = new SerialPort(SelectedAvailablePort, SelectedBaudRate);
                try
                {
                    COMPort.Open();
                    CmdConnectIsEnabled = false;
                    CmdDisConnectIsEnabled = true;
                    _runningTask = true;
                    ReadingSerialState();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void ReadingSerialState()
        {
            Thread _thread = null;
            var taskState = Task.Run(() =>
            {
                _thread = Thread.CurrentThread;
                while (true)
                {
                    Thread.Sleep(100);
                    if (!_runningTask)
                    {
                        StateOfDevice = "State: " + (COMPort.IsOpen ? "Connected!" : "Not connected!");

                        _thread.Abort();
                    }
                    StateOfDevice = "State: " + (COMPort.IsOpen ? "Connected!" : "Not connected!");

                }
            });
        }

        private void DisConnect()
        {
            CmdConnectIsEnabled = true;
            CmdDisConnectIsEnabled = false;
            _runningTask = false;
            ReadingSerialState();
            COMPort.Close();
        }

        private void SendData()
        {

            if (COMPort == null)
            {
                MessageBox.Show("Serial Port is not active!");
            }
            else
            {
                COMPort.Write(MessageSendText);

                COMPort.DataReceived += new SerialDataReceivedEventHandler(DataRecieved);
            }

        }

        private void DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            MessageRecievedText += COMPort.ReadExisting();
        }

        #region Properties

        public string SelectedAvailablePort
        {
            get
            {
                return _selectedAvailablePort;
            }

            set
            {
                _selectedAvailablePort = value;
                OnPropertyChanged("AvailablePorts");
            }
        }

        public int SelectedBaudRate
        {
            get
            {
                return _selectedBaudRate;
            }

            set
            {
                _selectedBaudRate = value;
                OnPropertyChanged("BaudRates");
            }
        }

        public List<string> AvailablePorts
        {
            get
            {
                return _availablePorts;
            }
            set
            {
                _availablePorts = value;
                OnPropertyChanged("AvailablePorts");

            }
        }

        public List<int> BaudRates
        {
            get
            {
                return _baudRates;
            }
            set
            {
                _baudRates = value;
                OnPropertyChanged("BaudRates");
            }
        }

        public string StateOfDevice
        {
            get
            {
                return _stateOfDevice;
            }
            set
            {
                _stateOfDevice = value;
                OnPropertyChanged("StateOfDevice");

            }
        }

        public string MessageSendText
        {
            get
            {
                return _messageSendText;
            }
            set
            {
                _messageSendText = value;
                OnPropertyChanged("MessageSendText");

            }
        }

        public string MessageRecievedText
        {
            get
            {
                return _messageRecievedText;
            }
            set
            {
                _messageRecievedText = value;
                OnPropertyChanged("MessageRecievedText");

            }
        }

        public bool CmdConnectIsEnabled
        {
            get
            {
                return _connectButtonIsEnabled; ;
            }
            set
            {
                _connectButtonIsEnabled = value;
                OnPropertyChanged("CmdConnectIsEnabled");
            }
        }

        public bool CmdDisConnectIsEnabled
        {
            get
            {
                return _disConnectButtonIsEnabled; ;
            }
            set
            {
                _disConnectButtonIsEnabled = value;
                OnPropertyChanged("CmdDisConnectIsEnabled");
            }
        }
        #endregion
    }
}