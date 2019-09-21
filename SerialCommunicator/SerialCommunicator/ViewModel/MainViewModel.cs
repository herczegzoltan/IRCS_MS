﻿using SerialCommunicator.Model;
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
using SerialCommunicator.Resource;
using System.Xml.Linq;
using System.Diagnostics;

namespace SerialCommunicator.ViewModel
{
    public class MainViewModel : NotifyViewModel
    {
        #region Variables
        private ICommand _connectCommand;

        private ICommand _disConnectCommand;

        private ICommand _measureOn;


        private ICommand _measureOff;

        private ICommand _run;

        private List<string> _availablePorts;

        private List<int> _baudRates;

        private string _selectedCardType = "";
        private string _selectedMeasureType = "";

        private List<string> _cardTypes;

        private List<string> _measureTypes;

        private string _selectedAvailablePort = null;

        private string _messageSendText;

        private string _messageRecievedText;

        private int _selectedBaudRate = 0;

        public ICommand CmdConnect => _connectCommand;

        public ICommand CmdDisConnect => _disConnectCommand;

        public ICommand CmdMeasureOn => _measureOn;

        public ICommand CmdMeasureOff => _measureOff;

        public ICommand CmdRun => _run;


        private string _stateOfDevice = "State: Not connected!";

        private bool _connectButtonIsEnabled = true;

        private bool _disConnectButtonIsEnabled;
        private bool _measureOffButtonIsEnabled;
        private bool _runButtonIsEnabled;

        private bool _runningTask;

        XmlFilter xmlData = null;
        SerialPort COMPort = null;
        private DateTime _currentDateTime;
        private bool _measureOnButtonIsEnabled;

        #endregion

        public MainViewModel()
        {
            _connectCommand = new DelegateCommand(ConnectToDevice);
            _disConnectCommand = new DelegateCommand(DisConnect);
            _measureOn = new DelegateCommand(SendMeasureOn);
            _measureOff = new DelegateCommand(SendMeasureOff);
            _run = new DelegateCommand(SendRun);

            AvailablePorts = SerialCommunicationSettings.ListOfSerialPorts();
            BaudRates = SerialCommunicationSettings.ListOfSerialBaudRates();

            xmlData = new XmlFilter();
            CardTypes = xmlData.GetCardTypeNames();

            UpdateTimeUI();

            CmdRunIsEnabled = false;
            CmdMeasureOffIsEnabled = false;
            CmdMeasureOnIsEnabled = false;
        }

        private void UpdateTimeUI() {

            Thread _thread = null;
            var taskState = Task.Run(() =>
            {
                _thread = Thread.CurrentThread;
                while (true)
                {
                    Thread.Sleep(100);

                    CurrentDateTime = DateTime.UtcNow;
                }
            });
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
                    COMPort.DataReceived += new SerialDataReceivedEventHandler(DataRecieved);
                    COMPort.Open();
                    CmdConnectIsEnabled = false;
                    CmdDisConnectIsEnabled = true;
                    _runningTask = true;
                    ReadingSerialState();
                    ConfigureDevice();
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
                    if (!_runningTask || !COMPort.IsOpen)
                    {
                        CmdConnectIsEnabled = true;
                        CmdDisConnectIsEnabled = false;

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
            DisConfigureDevice();
            //34,0,13


            //while (true)
            //{
            //    try
            //    {
            //        COMPort.DataReceived -= new SerialDataReceivedEventHandler(DataRecieved);
            //        COMPort.Close();

            //        break;
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }

            //}

        }

        private void ConfigureDevice()
        {

            ByteMessageBuilder.SetByteArray(0, xmlData.GetConnect());
            ByteMessageBuilder.SetByteArray(1, 0x00);
            ByteMessageBuilder.SetByteArray(2, 0x00);
            ByteMessageBuilder.SetByteArray(3, 0x00);
            ByteMessageBuilder.SetByteArray(4, xmlData.GetEOF());

            LoopMessagesArrayToSend();
        }

        private void SendMeasureOn()
        {

            ByteMessageBuilder.SetByteArray(0, xmlData.GetMeasureOn());
            ByteMessageBuilder.SetByteArray(1, 0x00);
            ByteMessageBuilder.SetByteArray(2, 0x00);
            ByteMessageBuilder.SetByteArray(3, 0x00);
            ByteMessageBuilder.SetByteArray(4, xmlData.GetEOF());

            LoopMessagesArrayToSend();

            CmdMeasureOffIsEnabled = true;
            CmdMeasureOnIsEnabled = false;
            CmdRunIsEnabled = true;

        }

        private void SendMeasureOff()
        {
            ByteMessageBuilder.SetByteArray(0, xmlData.GetMeasureOff());
            ByteMessageBuilder.SetByteArray(1, 0x00);
            ByteMessageBuilder.SetByteArray(2, 0x00);
            ByteMessageBuilder.SetByteArray(3, 0x00);
            ByteMessageBuilder.SetByteArray(4, xmlData.GetEOF());

            LoopMessagesArrayToSend();

            CmdMeasureOffIsEnabled = false;
            CmdMeasureOnIsEnabled = true;
            CmdRunIsEnabled = false;
        }

        private void SendRun()
        {
            ByteMessageBuilder.SetByteArray(0, xmlData.GetMeasureOn());
            ByteMessageBuilder.SetByteArray(1, xmlData.GetSelectedCardTypeValue(SelectedCardType));
            ByteMessageBuilder.SetByteArray(2, xmlData.GetSelectedMeasurementValue(SelectedCardType,SelectedMeasureType));
            ByteMessageBuilder.SetByteArray(3, xmlData.GetRun());
            ByteMessageBuilder.SetByteArray(4, xmlData.GetEOF());

            LoopMessagesArrayToSend();


            ///*
            //foreach (byte item in ByteMessageBuilder.GetByteList())
            //{
            //    SendData(item);
            //}*/
            //SendData(0x07);
            //SendData(0x03);
            //SendData(0x05); //run on
            //SendData(0x0D); //eof
        }

        private void LoopMessagesArrayToSend()
        {
            for (int i = 0; i < ByteMessageBuilder.GetByteArray().Length; i++)
            {
                SendData(ByteMessageBuilder.GetByteArray()[i]);
            }
        }

        private void DisConfigureDevice()
        {
            //Disconnect FIX
            SendData(0x02); //connect
            SendData(0x00); //connect
            SendData(0x00); //connect
            SendData(0x00); //connect
            SendData(0x0D); //connect
            //connect
        }

        private void SendData(byte data)
        {
            var dataArray = new byte[] { data };
            if (COMPort == null)
            {
                MessageBox.Show("Serial Port is not active!");
            }
            else
            {
                COMPort.Write(dataArray,0,1);
                COMPort.DataReceived += new SerialDataReceivedEventHandler(DataRecieved);

            }
        }

        private void DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {

            SerialPort sp = (SerialPort)sender;
            
            if (COMPort.IsOpen)
            {
                MessageRecievedText += (COMPort.ReadByte()).ToString() + "\n";
            }
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

        public string SelectedCardType
        {
            get
            {
                return _selectedCardType;
            }

            set
            {
                _selectedCardType = value;
                OnPropertyChanged("CardTypes");
                MeasureTypes = xmlData.GetMeasurements(SelectedCardType);
       
            }
        }

        public List<string> CardTypes
        {
            get
            {
                return _cardTypes;
            }
            set
            {
                _cardTypes = value;
                OnPropertyChanged("CardTypes");
            }
        }

        public string SelectedMeasureType
        {
            get
            {
                return _selectedMeasureType;
            }

            set
            {
                _selectedMeasureType = value;
                OnPropertyChanged("MeasureTypes");

                if (value != null)
                {
                    CmdMeasureOnIsEnabled = true;
                }
            }
        }

        public List<string> MeasureTypes
        {
            get
            {
                return _measureTypes;
            }
            set
            {
                _measureTypes = value;
                OnPropertyChanged("MeasureTypes");
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

        public DateTime CurrentDateTime
        {
            get
            {
                return _currentDateTime;
            }
            set
            {
                _currentDateTime = value;
                OnPropertyChanged("CurrentDateTime");

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

        /// 

        public bool CmdMeasureOnIsEnabled
        {

            get
            {
                return _measureOnButtonIsEnabled; ;
            }
            set
            {
                _measureOnButtonIsEnabled = value;
                OnPropertyChanged("CmdMeasureOnIsEnabled");
            }
        }
        public bool CmdMeasureOffIsEnabled
        {

            get
            {
                return _measureOffButtonIsEnabled; ;
            }
            set
            {
                _measureOffButtonIsEnabled = value;
                OnPropertyChanged("CmdMeasureOffIsEnabled");
            }
        }
        public bool CmdRunIsEnabled
        {
            get
            {
                return _runButtonIsEnabled; ;
            }
            set
            {
                _runButtonIsEnabled = value;
                OnPropertyChanged("CmdRunIsEnabled");
            }
        }
        #endregion
    }
}