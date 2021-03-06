﻿using IRCS_MS.Model;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IRCS_MS.Infrastructure.Message;
using System.IO.Ports;
using System.Windows;
using System.Threading;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using IRCS_MS.Helper;
using System.ComponentModel;
using IRCS_MS.ViewModel.Commands;
using IRCS_MS.Infrastructure.XmlHandler;
using IRCS_MS.Infrastructure;
using IRCS_MS.ViewModel.MainViewModelCommands;
using System.Net.Sockets;
using System.Net;
using System.Timers;
using IRCS_MS.Infrastructure.MeasureMode;

namespace IRCS_MS.ViewModel
{

    public class MeasureModeViewModel : INotifyPropertyChanged
    {
        public ConnectCommand ConnectCommand { get; set; }
        public DisConnectCommand DisConnectCommand { get; set; }
        public MeasureOffCommand MeasureOffCommand { get; set; }
        public MeasureOnCommand MeasureOnCommand { get; set; }
        public RunCommand RunCommand { get; set; }

        #region Variables
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

        private string _stateOfDevice = "State: Not connected!";

        private string _stateOfDeviceColor = "Red";

        private bool _connectButtonIsEnabled = true;

        private bool _disConnectButtonIsEnabled;

        private bool _measureOffButtonIsEnabled;

        private bool _measureOnButtonIsEnabled;

        private bool _cmdCardTypeIsEnabled;

        private bool _cmdMeasureTypeIsEnabled;

        private bool _runButtonIsEnabled;

        private bool _runningTask;

        private string _currentDateTime;

        public bool WasItRun = false;

        private int countBytes = 0;

        private ulong _schauerNumber;
        
        private string _currentMeasureCount = "Measured data to save: 0";
        private bool _reportFieldState;
        private bool _reportCheckBoxEnabled;

        private Stopwatch _stopWatchTimeOut =null;

        #region UDP controlls 

        private static UdpClient CTRL_udpClient;
        private static System.Timers.Timer connectionTimer = new System.Timers.Timer(2000);

        private static int tries = 15;
        #endregion

        public UIElementCollectionHelper  UIElementCollectionHelper{ get; set; }

        #endregion
        public MeasureModeViewModel()
        {

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            ConnectCommand = new ConnectCommand(this);
            DisConnectCommand = new DisConnectCommand(this);
            MeasureOffCommand = new MeasureOffCommand(this);
            MeasureOnCommand = new MeasureOnCommand(this);
            RunCommand = new RunCommand(this);

            AvailablePorts = SerialCommunicationSettings.ListOfSerialPorts();
            BaudRates = SerialCommunicationSettings.ListOfSerialBaudRates();

            CardTypes = XmlFilter.Instance.GetCardTypeNames();

            UpdateTimeUI();
            UIElementCollectionHelper = new UIElementCollectionHelper(this);
            UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.ConnectBeforeClick);
            
            ReadingSerialState();
            
            ReportDataCollector.InitializeLists();
            IsRunningNow = GeneralMessageCollection.IsRunningStateChecker(false);
            _stopWatchTimeOut = new Stopwatch();

        }

        private void MeasureTypeComboBoxChanged()
        {
            if (SelectedMeasureType == "AutoMeasure")
            {
                ReportFieldState = false;
                ReportCheckBoxEnabled = true;
            }
            else
            {
                ReportFieldState = false;
                ReportCheckBoxEnabled = false;
            }
        }

      
        private void UpdateTimeUI()
        {
            Thread _thread = null;
            var taskState = Task.Run(() =>
            {
                _thread = Thread.CurrentThread;
                while (true)
                {
                    Thread.Sleep(100);

                    string s = DateTime.Now.ToString("yyyy MMMM dd, dddd") + " " + DateTime.Now.ToString("HH:mm:ss");

                    CurrentDateTime = s;//DateTime.UtcNow;
                }
            });
        }

        public void ConnectToDevice()
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
                SerialPortManager.Instance.BaudRate = SelectedBaudRate;
                SerialPortManager.Instance.PortName = SelectedAvailablePort;
                try
                {
                    SerialPortManager.Instance.Open();

                    if (CTRL_udpClient == null)
                    {
                        CTRL_udpClient = new UdpClient(23999);
                        CTRL_udpClient.BeginReceive(OnCTRL_UDPReceive, null);
                    }

                    UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.ConnectAfterClick);
                    _runningTask = true;
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
            var taskState = Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(500);

                    if (_runningTask)
                    {
                        StateOfDevice = "State: " + (SerialPortManager.Instance.IsOpen? "Connected!" : "Not connected!");
                        StateOfDeviceColor = (SerialPortManager.Instance.IsOpen? "Green" : "Red");
                    }

                }
            });
        }

        #region Commands

        public void DisConnect()
        {
            DisConfigureDevice();
            UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.DisConnectClick);
        }
        private void ConfigureDevice()
        {

            MeasureModeByteMessagesStandardCommands.ConnectConfigureDevice();
            WasItRun = false;

            LoopMessagesArrayToSend();
        }

        public void SendMeasureOn()
        {
            MeasureModeByteMessagesStandardCommands.MeasureOn();

            WasItRun = false;
            LoopMessagesArrayToSend();
            UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.MeasureOnAfterClick);
        }

        public void SendMeasureOff()
        {
            MeasureModeByteMessagesStandardCommands.MeasureOff();

            WasItRun = false;
            LoopMessagesArrayToSend();
            UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.MeasureOffClick);
        }

        public void SendRun()
        {
            //stopwatch
            TimeOutValidator(TimeOutValidatorStates.Start);
            //stopWatchTimeOut.Start();

            MeasureModeByteMessagesStandardCommands.SendRun(SelectedCardType, SelectedMeasureType);
   
            WasItRun = true;
            LoopMessagesArrayToSend();
        }

        #endregion

        private void LoopMessagesArrayToSend()
        {
            ByteMessageBuilderRepository.ClearArray(ByteMessages.Instance.MeasureModeIncoming);

            for (int i = 0; i < ByteMessages.Instance.MeasureModeOutgoing.Length; i++)
            {
                SendData(ByteMessages.Instance.MeasureModeOutgoing[i]);
            }
        }

        private void DisConfigureDevice()
        {
            MeasureModeByteMessagesStandardCommands.DisConfigureDevice();
            WasItRun = false;
            LoopMessagesArrayToSend();
        }

        private void SendData(byte data)
        {
            var dataArray = new byte[] { data };
            if (SerialPortManager.Instance == null)
            {
                MessageBox.Show("Serial Port is not active!");
            }
            else
            {
                SerialPortManager.Instance.DataReceived += new SerialDataReceivedEventHandler(DataRecieved);
                SerialPortManager.Instance.Write(dataArray, 0, 1);
            }
        }

        private void PopUpQuestionbox()
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you want new card?", "Card Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
            {
                WasItRun = false;
                _savedMeasureCounter = 0;
                FolderDialog();
            }
            else
            {
                WasItRun = false;
                SchauerNumber++;
            }
        }

        private void TopMessage(string header, string text)
        {
            MessageBoxWrapper.Show(text, header, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void TimeOutValidator(TimeOutValidatorStates tovs)
        {
            switch (tovs)
            {
                case TimeOutValidatorStates.Start:
                    TaskTimeOutWatcher();
                    break;
                case TimeOutValidatorStates.Reset:
                    _stopWatchTimeOut.Restart();

                    break;
                case TimeOutValidatorStates.Stop:
                    _stopWatchTimeOut.Stop();
                    break;
                default:
                    break;
            }
        }

        private bool TaskTimeOutWatcherIsRunning = false;
        private void TaskTimeOutWatcher()
        {
            if (!TaskTimeOutWatcherIsRunning)
            {
                _stopWatchTimeOut.Start();

                var taskState = Task.Run(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(100);

                        if (_stopWatchTimeOut.ElapsedMilliseconds > XmlFilter.Instance.GetDefaultTimeOutValue())
                        {
                            MessageBox.Show($"TimeOut error! > {XmlFilter.Instance.GetDefaultTimeOutValue()} ms");
                            TimeOutValidator(TimeOutValidatorStates.Reset);
                        }
                    }
                });

                TaskTimeOutWatcherIsRunning = true;
            }
        }

        private int _counterIncomingPackage = 1;
        private int _extramessages = 0;
        private bool _validateFinished = false;
        private int _savedMeasureCounter = 0;

        private void DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            if (SerialPortManager.Instance.IsOpen)
            {
                try
                {
                    string incomingByte = SerialPortManager.Instance.ReadByte().ToString();

                    ByteMessageBuilderRepository.SetStrArrayByIndex(ByteMessages.Instance.MeasureModeIncoming, countBytes, incomingByte);
                    
                    //all bytes arrived
                    if (countBytes == 2)
                    {
                        if (WasItRun)
                        {

                            IsRunningNow = GeneralMessageCollection.IsRunningStateChecker(true);

                            TimeOutValidator(TimeOutValidatorStates.Reset);

                            if (ValidatorIncomingMessage.ValidationEOF())
                            {
                                _extramessages = XmlFilter.Instance.IsCommonIncluded(SelectedCardType) == true ?
                                    XmlFilter.Instance.GetNumberOfExpectedMeasureState(XmlFilter.Instance.GetDefaultName()) * XmlFilter.Instance.DefaultNumbersOfBytes :
                                    _extramessages = 0;


                                //if the incoming messages's number is equle with the required number from XML file
                                if (_counterIncomingPackage ==
                                    XmlFilter.Instance.GetNumberOfExpectedMeasureState(SelectedCardType) * XmlFilter.Instance.DefaultNumbersOfBytes + _extramessages)
                                {
                                    TimeOutValidator(TimeOutValidatorStates.Stop);

                                    MessageRecievedText = GeneralMessageCollection.GeneralMessageRecivedTranslation(" -> Validate OK") + MessageRecievedText;
                                    //_counterIncomingPackage = 1;
                                    _validateFinished = true;
                                    GeneralMessageCollection.LoopCounter = 0;
                                }
                                else
                                {
                                 //when it is not validated yet.   
                                    TimeOutValidator(TimeOutValidatorStates.Reset);
                                    MessageRecievedText = GeneralMessageCollection.GeneralMessageRecivedTranslation("") + MessageRecievedText;
                                    GeneralMessageCollection.LoopCounter++;
                                    _validateFinished = false;
                                }

                                //if incoming message returns with measure ok or not-> negative logic
                                if (ValidatorIncomingMessage.ErrorMessageBack(ByteMessages.Instance.MeasureModeIncoming[1]))
                                {
                                    //TimeOutValidator(TimeOutValidatorStates.Reset);
                                    TimeOutValidator(TimeOutValidatorStates.Stop);
                                    //_counterIncomingPackage = 1;
                                    _validateFinished = true;
                                    GeneralMessageCollection.LoopCounter = 0;
                                }

                                if (ReportFieldState)
                                {
                                    _savedMeasureCounter++;

                                    string reportInsertData = XmlFilter.Instance.GetResponseData(
                                        ConverterRepository.ConvertDecimalStringToHexString(ByteMessages.Instance.MeasureModeIncoming[1].ToString()));

                                    ReportDataCollector.AddToVertical(reportInsertData);

                                    if (ReportFieldState && _validateFinished)
                                    {
                                        ReportDataCollector.AddToVerticalAtIndex(0, SchauerNumber.ToString());
                                        ReportDataCollector.AddVerticalToHorizontal();
                                        ReportDataCollector.CleanerVertical();
                                        PopUpQuestionbox();
                                    }
                                }
                            }
                            else
                            {
                                //TimeOutValidator(TimeOutValidatorStates.Reset);
                                TimeOutValidator(TimeOutValidatorStates.Stop);
                                //_counterIncomingPackage = 1;
                                _validateFinished = true;
                                GeneralMessageCollection.LoopCounter = 0; 
                                MessageRecievedText = GeneralMessageCollection.GeneralMessageRecived("Validate Error -> Wrong EoF") + MessageRecievedText;
                            }
                        }
                        else
                        {
                            MessageRecievedText = "Info: " + DateTime.Now.ToString("HH:mm:ss").ToString() + " -> " +
                                                XmlFilter.Instance.GetResponseTranslate
                                                (ByteMessages.Instance.MeasureModeIncoming[0].ToString(),
                                                ByteMessages.Instance.MeasureModeIncoming[1].ToString(),
                                                ByteMessages.Instance.MeasureModeIncoming[2].ToString())
                                                + "\n" + MessageRecievedText + "\n";
                        }

                        countBytes = 0;
                        WasItDisconnect();
                        ByteMessageBuilderRepository.ClearArray(ByteMessages.Instance.MeasureModeIncoming);
                    }
                    else
                    {
                        countBytes++;
                    }

                    if (WasItRun && !_validateFinished)
                    {
                        _counterIncomingPackage++;
                    }
                    if(_validateFinished)
                    {
                        _counterIncomingPackage = 1;
                        _validateFinished = false;
                        IsRunningNow = GeneralMessageCollection.IsRunningStateChecker(false);
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                    MessageBox.Show(GeneralMessageCollection.LogIntoFile(ex));
                }
            }
        }

        private void WasItDisconnect()
        {
            if (XmlFilter.Instance.GetResponseTranslate
                                               (ByteMessages.Instance.MeasureModeIncoming[0].ToString(),
                                               ByteMessages.Instance.MeasureModeIncoming[1].ToString(),
                                               ByteMessages.Instance.MeasureModeIncoming[2].ToString()) == "Disconnected")
            {
                SerialPortManager.Instance.Close();
                SerialPortManager.Instance.Dispose();
            }
        }

        private void SaveReport()
        {
            if (ReportDataCollector.GetTotal().Any())
            {
               if (FolderPath != "")
               {
                    string FileName = $"IRCS_{SelectedCardType}_{ReportDataCollector.GetTotal().First().ElementAt(0)}_"+
                      $"{ulong.Parse(ReportDataCollector.GetTotal().Last().ElementAt(0)) - ulong.Parse(ReportDataCollector.GetTotal().First().ElementAt(0)) + 1}";

                    //"IRCS_"CardName"_"kezdőszám"_"hány darab kártya lett mérve".xls;
                    ReportDataHelper.InitializeMeasure(FileName, FolderPath);

                    if (XmlFilter.Instance.GetMeasurementsWithoutAutoMeasure(SelectedCardType).Count == 0)
                    {
                        //only automeasure 
                        ReportDataHelper.PassListTOReport(

                        XmlFilter.Instance.GetMeasurements(XmlFilter.Instance.GetDefaultName()), ReportDataCollector.GetTotal(), Name, new List<string>() { });
                    }
                    else
                    {
                        if (XmlFilter.Instance.IsCommonIncluded(SelectedCardType))
                        {
                            ReportDataHelper.PassListTOReport(

                                XmlFilter.Instance.GetMeasurementsWithoutAutoMeasure(XmlFilter.Instance.GetDefaultName())
                                    .Concat(XmlFilter.Instance.GetMeasurementsWithoutAutoMeasure(SelectedCardType))
                                    .ToList()

                                , ReportDataCollector.GetTotal(), Name, ReportDataCollector.FillColumnForReport(true,SelectedCardType));
                        }
                        else
                        {
                            ReportDataHelper.PassListTOReport(XmlFilter.Instance.GetMeasurementsWithoutAutoMeasure(SelectedCardType), ReportDataCollector.GetTotal(), Name, ReportDataCollector.FillColumnForReport(false,SelectedCardType));
                        }
                    }

                    ReportDataHelper.CreateReportFile();

                    TopMessage("Saving File....", "File Saved!");
                }
            }
            else
            {
                TopMessage("Error!", "No measurement data!");
            }
        }

      
        private string FolderPath = "";
        private string _isRunningNow;
        private string _name;

        private void FolderDialog()
        {
            string selectedPath;

            var t = new Thread((ThreadStart)(() => {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowNewFolderButton = true;
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    FolderPath = "";
                }
                if (result == DialogResult.OK)
                {
                    selectedPath = fbd.SelectedPath;
                    FolderPath = selectedPath;
                    SaveReport();
                }

            }));

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

        }
        #region VoIP_methods

        private void SendKeepalive()
        {
            int command_counter = 11;
            byte[] counterBytes = Encoding.ASCII.GetBytes(command_counter.ToString());

            byte[] commandBytes = Encoding.ASCII.GetBytes("KEEPALIVE");
            byte[] sendBytes = new byte[counterBytes.Length + commandBytes.Length];

            Array.Copy(counterBytes, 0, sendBytes, 0, counterBytes.Length);
            Array.Copy(commandBytes, 0, sendBytes, counterBytes.Length, commandBytes.Length);
            IPEndPoint voipend = new IPEndPoint(IPAddress.Parse("192.168.1.122"), 23400);
            CTRL_udpClient.Send(sendBytes, sendBytes.Length, voipend);

        }

        private void IP_loopback()
        {
            int command_counter = 11;
            byte[] counterBytes = Encoding.ASCII.GetBytes(command_counter.ToString());

            byte[] commandBytes = Encoding.ASCII.GetBytes("KEEPALIVE");
            byte[] sendBytes = new byte[counterBytes.Length + commandBytes.Length];

            Array.Copy(counterBytes, 0, sendBytes, 0, counterBytes.Length);
            Array.Copy(commandBytes, 0, sendBytes, counterBytes.Length, commandBytes.Length);
            IPEndPoint voipend = new IPEndPoint(IPAddress.Parse("192.168.1.122"), 23400);
            CTRL_udpClient.Send(sendBytes, sendBytes.Length, voipend);

        }

        private void OnCTRL_UDPReceive(IAsyncResult res)
        {
            //nodataneeded = false;
            // UdpClient u = (UdpClient)((UdpState)(res.AsyncState)).u;
            //IPEndPoint e = (IPEndPoint)((UdpState)(res.AsyncState)).e;
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            if (CTRL_udpClient != null)
            {
                Byte[] receiveBytes = CTRL_udpClient.EndReceive(res, ref remote);
                string receiveString = Encoding.ASCII.GetString(receiveBytes);

                if (receiveString.Contains("KEEPALIVE"))
                {
                    connectionTimer.Elapsed -= OnTimedEvent;
                    connectionTimer.Enabled = false;
                    UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.MeasureOnAfterClick);
                }

                MessageRecievedText += " Received:" + receiveString.ToString() + "\r\n";

                CTRL_udpClient.BeginReceive(new AsyncCallback(OnCTRL_UDPReceive), null);
            }
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            MessageRecievedText = "VoIP Connecting... Timeout in " + (tries * 2).ToString() + "s";
            int command_counter = 11;
            byte[] counterBytes = Encoding.ASCII.GetBytes(command_counter.ToString());

            byte[] commandBytes = Encoding.ASCII.GetBytes("KEEPALIVE");
            byte[] sendBytes = new byte[counterBytes.Length + commandBytes.Length];

            Array.Copy(counterBytes, 0, sendBytes, 0, counterBytes.Length);
            Array.Copy(commandBytes, 0, sendBytes, counterBytes.Length, commandBytes.Length);
            IPEndPoint voipend = new IPEndPoint(IPAddress.Parse("192.168.1.122"), 23400);
            CTRL_udpClient.Send(sendBytes, sendBytes.Length, voipend);
            if (tries-- == 0)
            {
                connectionTimer.Enabled = false;
                connectionTimer.Elapsed -= OnTimedEvent;
                MessageRecievedText += "\r\nVoIP connecting error! Try RUN.\r\n";
                UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.MeasureOnAfterClick);
            }
        }

        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
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
                MeasureTypes = XmlFilter.Instance.GetMeasurements(SelectedCardType);
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
                MeasureTypeComboBoxChanged();
                if (value != null)
                {
                    UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.CardAndMeasureSelected);
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

        public string StateOfDeviceColor
        {
            get
            {
                return _stateOfDeviceColor;
            }
            set
            {
                _stateOfDeviceColor = value;
                OnPropertyChanged("StateOfDeviceColor");

            }
        }

        public string CurrentDateTime
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

        public bool CmdCardTypeIsEnabled
        {
            get
            {
                return _cmdCardTypeIsEnabled;
            }
            set
            {
                _cmdCardTypeIsEnabled = value;
                OnPropertyChanged("CmdCardTypeIsEnabled");
            }
        }

        public bool CmdMeasureTypeIsEnabled
        {
            get
            {
                return _cmdMeasureTypeIsEnabled;
            }
            set
            {
                _cmdMeasureTypeIsEnabled = value;
                OnPropertyChanged("CmdMeasureTypeIsEnabled");
            }
        }

        public ulong SchauerNumber
        {
            get
            {
                if (_schauerNumber.ToString().Length == 18)
                {
                    MessageBox.Show("Maximum Schauer Number is reached!");
                }

                return _schauerNumber;
            }
            set
            {

                _schauerNumber = value;
                OnPropertyChanged("SchauerNumber");
            }
        }


        public string CurrentMeasureCount
        {
            get
            {
                return _currentMeasureCount;
            }
            set
            {
                _currentMeasureCount = value;
                OnPropertyChanged("CurrentMeasureCount");

            }
        }


        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name= value;
                OnPropertyChanged("Name");

            }
        }


        public bool ReportFieldState
        {
            get
            {
                return _reportFieldState;
            }
            set
            {
                _reportFieldState = value;
                OnPropertyChanged("ReportFieldState");
            }
        }


        public bool ReportCheckBoxEnabled
        {
            get
            {
                return _reportCheckBoxEnabled;
            }
            set
            {
                _reportCheckBoxEnabled = value;
                OnPropertyChanged("ReportCheckBoxEnabled");
            }
        }


        public string IsRunningNow
        {
            get
            {
                return _isRunningNow;
            }
            set
            {
                _isRunningNow = value;
                OnPropertyChanged("IsRunningNow");
            }
        }


        #endregion
    }
}