using IRCS_MS.Model;
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
using System.Net.NetworkInformation;
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

        private int _incomingMeasureCounter = 1;
        private int _commonMeasures = 3;
        private bool _validateFinished = false;
        private int _savedMeasureCounter = 0;

        private Stopwatch _stopWatchTimeOut = null;

        #region UDP controlls 

        private static UdpClient CTRL_udpClient;
        private static UdpClient Ch1_udpClient;
        private static UdpClient Ch2_udpClient;
        private static System.Timers.Timer keepaliveTimer = new System.Timers.Timer(10000);

        private Udp _udp;
        private byte[] _receivedBytes = new byte[3];


        #endregion

        public UIElementCollectionHelper UIElementCollectionHelper { get; set; }

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
            this._udp = new Udp();
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
                        CTRL_udpClient = new UdpClient(23400);
                        CTRL_udpClient.BeginReceive(OnCTRL_UDPReceive, null);
                    }
                    if (Ch1_udpClient == null)
                    {
                        Ch1_udpClient = new UdpClient(23410);
                        Ch1_udpClient.BeginReceive(Ch1_UDPReceive, null);
                    }
                    if (Ch2_udpClient == null)
                    {
                        Ch2_udpClient = new UdpClient(23420);
                        Ch2_udpClient.BeginReceive(Ch2_UDPReceive, null);
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
                        StateOfDevice = "State: " + (SerialPortManager.Instance.IsOpen ? "Connected!" : "Not connected!");
                        StateOfDeviceColor = (SerialPortManager.Instance.IsOpen ? "Green" : "Red");
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
            MeasureModeByteMessagesStandardCommands.MeasureOn(SelectedCardType, SelectedMeasureType);

            WasItRun = false;
            LoopMessagesArrayToSend();
            UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.MeasureOnAfterClick);
            if (SelectedCardType == "VoIP2CH")
            {
                UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.MeasureOnAfterClickVoip);
                TopMessage("Keepalive", "Wait until Voip gets Keepalive!");
                keepaliveTimer.Elapsed += KeepaliveSendEvent;
                keepaliveTimer.AutoReset = true;
                keepaliveTimer.Enabled = true;
            }
        }

        public void SendMeasureOff()
        {
            MeasureModeByteMessagesStandardCommands.MeasureOff();
            WasItRun = false;
            LoopMessagesArrayToSend();
            UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.MeasureOffClick);
            keepaliveTimer.Enabled = false;
        }
        public void SendResetOk()
        {
            MeasureModeByteMessagesStandardCommands.ResetOk();
            WasItRun = true;
            LoopMessagesArrayToSend();
        }

        public void SendResetNok()
        {
            MeasureModeByteMessagesStandardCommands.ResetNok();
            WasItRun = true;
            LoopMessagesArrayToSend();
        }

        public void SendRun()
        {
            TimeOutValidator(TimeOutValidatorStates.Start);
            MeasureModeByteMessagesStandardCommands.SendRun(SelectedCardType, SelectedMeasureType);
            WasItRun = true;
            LoopMessagesArrayToSend();
        }

        public void UdpToUartTransmitStart(char data)
        {
            MeasureModeByteMessagesStandardCommands.UdpUartTransmitStart(SelectedCardType, SelectedMeasureType, data);
            WasItRun = true;
            LoopMessagesArrayToSend();
        }

        public void UdpToUartTransmitStop()
        {
            MeasureModeByteMessagesStandardCommands.UdpUartTransmitStop(SelectedCardType, SelectedMeasureType);
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

        private void DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            if (SerialPortManager.Instance.IsOpen)
            {
                try
                {
                    byte received_byte = Convert.ToByte(SerialPortManager.Instance.ReadByte());

                    _receivedBytes[this.countBytes] = received_byte;
                    string incomingByte = received_byte.ToString();

                    ByteMessageBuilderRepository.SetStrArrayByIndex(ByteMessages.Instance.MeasureModeIncoming, countBytes, incomingByte);

                    //all bytes arrived
                    if (countBytes == 2)
                    {
                        countBytes = 0;
                        if (WasItRun)
                        {

                            IsRunningNow = GeneralMessageCollection.IsRunningStateChecker(true);

                            TimeOutValidator(TimeOutValidatorStates.Reset);

                            if (ValidatorIncomingMessage.ValidationEOF())
                            {
                                if (XmlFilter.Instance.IsCommonIncluded(SelectedCardType) == true)
                                { _commonMeasures = XmlFilter.Instance.GetNumberOfExpectedMeasureState(XmlFilter.Instance.GetDefaultName()); }
                                else { _commonMeasures = 0; }

                                if (XmlFilter.Instance.GetResponseCommand(this._receivedBytes[0].ToString()) == "VOIP_UDP_Start")
                                {
                                    this._udp = new Udp();
                                    this._udp.IsEnabled = true;

                                    this._udp.SendBytes.Add(_receivedBytes[1]);
                                }
                                else if (XmlFilter.Instance.GetResponseCommand(this._receivedBytes[0].ToString()) == "VOIP_UDP_Cont")
                                {
                                    this._udp.IsEnabled = true;
                                    this._udp.SendBytes.Add(this._receivedBytes[1]);
                                }
                                else if (XmlFilter.Instance.GetResponseCommand(_receivedBytes[0].ToString()) == "VOIP_UDP_Stop")
                                {
                                    this._udp.IsEnabled = true;
                                    CTRL_udpClient.Send(this._udp.SendBytes.ToArray(), this._udp.SendBytes.Count, this._udp.IPADDRESS, this._udp.PORT);

                                    this._udp.IsFinished = true;
                                }

                                if (!this._udp.IsEnabled)
                                {
                                    //if the incoming messages's number is equle with the required number from XML file
                                    if (_incomingMeasureCounter == (XmlFilter.Instance.GetNumberOfExpectedMeasureState(SelectedCardType)) + _commonMeasures)
                                    {
                                        TimeOutValidator(TimeOutValidatorStates.Stop);
                                        MessageRecievedText = GeneralMessageCollection.GeneralMessageRecivedTranslation(" -> Validate OK") + MessageRecievedText;
                                        _validateFinished = true;
                                        GeneralMessageCollection.LoopCounter = 0;
                                    }
                                    else
                                    {
                                        if (SelectedMeasureType != "AutoMeasure")
                                        {
                                            IEnumerable<String> _measureList = XmlFilter.Instance.GetMeasureListByCardTypeWithoutAuto(SelectedCardType);

                                            var index = _measureList.ToList().IndexOf(SelectedMeasureType);
                                            GeneralMessageCollection.LoopCounter = index;
                                            TimeOutValidator(TimeOutValidatorStates.Stop);
                                            MessageRecievedText = GeneralMessageCollection.GeneralMessageRecivedTranslation("") + MessageRecievedText;
                                            GeneralMessageCollection.LoopCounter = 0;
                                            _validateFinished = false;

                                        }
                                        else
                                        {
                                            //when it is not validated yet and AutoMeasure.   
                                            TimeOutValidator(TimeOutValidatorStates.Reset);
                                            _incomingMeasureCounter++;
                                            MessageRecievedText = GeneralMessageCollection.GeneralMessageRecivedTranslation("") + MessageRecievedText;
                                            GeneralMessageCollection.LoopCounter++;
                                            _validateFinished = false;
                                        }
                                    }
                                }

                                //if incoming message returns with measure ok or not-> negative logic
                                if (IsMeasureModeIncomingReseted
                                    && ValidatorIncomingMessage.ErrorMessageBack(ByteMessages.Instance.MeasureModeIncoming[1])
                                    && !this._udp.IsEnabled)
                                {
                                    TimeOutValidator(TimeOutValidatorStates.Stop);
                                    _validateFinished = true;
                                    GeneralMessageCollection.LoopCounter = 0;
                                }
                                if (ReportFieldState && !this._udp.IsEnabled)
                                {
                                    _savedMeasureCounter++;

                                    string reportInsertData = IsMeasureModeIncomingReseted ? XmlFilter.Instance.GetResponseData(
                                        ConverterRepository.ConvertDecimalStringToHexString(ByteMessages.Instance.MeasureModeIncoming[1].ToString())) : "";

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
                                TimeOutValidator(TimeOutValidatorStates.Stop);
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
                        WasItDisconnect();
                        ByteMessageBuilderRepository.ClearArray(ByteMessages.Instance.MeasureModeIncoming);
                    }
                    else
                    {
                        countBytes++;
                    }

                    if (_validateFinished)
                    {
                        _incomingMeasureCounter = 1;
                        _validateFinished = false;
                        IsRunningNow = GeneralMessageCollection.IsRunningStateChecker(false);
                    }
                    if (this._udp.IsFinished)
                    {
                        this._udp.Reset();
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
            if (IsMeasureModeIncomingReseted
                && XmlFilter.Instance.GetResponseTranslate(
                    ByteMessages.Instance.MeasureModeIncoming[0].ToString(),
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
                     string FileName = $"IRCS_{SelectedCardType}_{ReportDataCollector.GetTotal().First().ElementAt(0)}_" +
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

                                , ReportDataCollector.GetTotal(), Name, ReportDataCollector.FillColumnForReport(true, SelectedCardType));
                        }
                        else
                        {
                            ReportDataHelper.PassListTOReport(XmlFilter.Instance.GetMeasurementsWithoutAutoMeasure(SelectedCardType), ReportDataCollector.GetTotal(), Name, ReportDataCollector.FillColumnForReport(false, SelectedCardType));
                        }
                    }

                    ReportDataHelper.CreateReportFile();


                    // Cleaning excel temp values
                    ReportDataCollector.ClearAll();

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

            var t = new Thread((ThreadStart)(() =>
            {
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
        private void KeepaliveSendEvent(object source, ElapsedEventArgs e)
        {
            string _keepalive = "00KEEPALIVE";
            byte[] sendbytes = Encoding.ASCII.GetBytes(_keepalive);

            if (CTRL_udpClient != null)
            {
                CTRL_udpClient.Send(sendbytes, sendbytes.Length, this._udp.IPADDRESS, this._udp.PORT);
            }
        }
        private void OnCTRL_UDPReceive(IAsyncResult res)
        {
            IPEndPoint voip_endpoint = new IPEndPoint(IPAddress.Parse("192.168.1.122"), 23400);
            if (CTRL_udpClient != null)
            {
                Byte[] receiveBytes = CTRL_udpClient.EndReceive(res, ref voip_endpoint);
                string receiveString = Encoding.ASCII.GetString(receiveBytes);
                if (receiveString.Contains("KEEPALIVE"))
                {
                    UIElementCollectionHelper.UIElementVisibilityUpdater(UIElementStateVariations.MeasureOnAfterClick);
                }
                else if (receiveString.Contains("T_MS_UDP"))
                {
                    string testString = "T_MS_UDP";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();

                }
                else if (receiveString.Contains("T_MS_EEPROM_OK"))
                {
                    string testString = "T_MS_EEPROM_OK";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("T_MS_EEPROM_NOK"))
                {
                    string testString = "T_MS_EEPROM_NOK";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("SENT"))
                {
                    string testString = "SENT";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("GPIO_OK"))
                {
                    string testString = "GPIO_OK";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("GPIO_ERROR"))
                {
                    string testString = "GPIO_ERROR";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("T_MS_FAN_10_OK"))
                {
                    string testString = "T_MS_FAN_10_OK";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("T_MS_FAN_10NOK"))
                {
                    string testString = "T_MS_FAN_10NOK";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("T_MS_FAN_11_OK"))
                {
                    string testString = "T_MS_FAN_11_OK";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("T_MS_FAN_11NOK"))
                {
                    string testString = "T_MS_FAN_11NOK";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("T_MS_FAN_20_OK"))
                {
                    string testString = "T_MS_FAN_20_OK";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("T_MS_FAN_20NOK"))
                {
                    string testString = "T_MS_FAN_20NOK";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("T_MS_FAN_21_OK"))
                {
                    string testString = "T_MS_FAN_21_OK";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else if (receiveString.Contains("T_MS_FAN_21NOK"))
                {
                    string testString = "T_MS_FAN_21NOK";
                    byte[] testArray = Encoding.UTF8.GetBytes(testString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();
                }
                else
                {
                    /*byte[] testArray = Encoding.UTF8.GetBytes(receiveString);

                    for (int i = 0; i < testArray.Length; i++)
                    {
                        UdpToUartTransmitStart(Convert.ToChar(testArray[i]));
                    }
                    UdpToUartTransmitStop();*/
                }

                MessageRecievedText += " Received:" + receiveString.ToString() + "\r\n";

                CTRL_udpClient.BeginReceive(new AsyncCallback(OnCTRL_UDPReceive), null);
            }
        }

        private void Ch1_UDPReceive(IAsyncResult res)
        {
            IPEndPoint voip_endpoint = new IPEndPoint(IPAddress.Parse("192.168.1.122"), 23410);
            if (CTRL_udpClient != null)
            {
                Byte[] receiveBytes = Ch1_udpClient.EndReceive(res, ref voip_endpoint);

                Ch1_udpClient.Send(receiveBytes, receiveBytes.Length, voip_endpoint);
            }

            Ch1_udpClient.BeginReceive(new AsyncCallback(Ch1_UDPReceive), null);

        }

        private void Ch2_UDPReceive(IAsyncResult res)
        {
            IPEndPoint voip_endpoint = new IPEndPoint(IPAddress.Parse("192.168.1.122"), 23420);
            if (CTRL_udpClient != null)
            {
                Byte[] receiveBytes = Ch2_udpClient.EndReceive(res, ref voip_endpoint);

                Ch2_udpClient.Send(receiveBytes, receiveBytes.Length, voip_endpoint);
            }
            Ch2_udpClient.BeginReceive(new AsyncCallback(Ch2_UDPReceive), null);
        }
        #endregion

        #region Properties
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// True if measure incoming is reseted (contains valid values).
        /// False if measure incoming is not reseted (does not contains valid values).
        /// </summary>
        private bool IsMeasureModeIncomingReseted
        {
            get
            {
                return ByteMessages.Instance.MeasureModeIncoming[0] != null
                && ByteMessages.Instance.MeasureModeIncoming[1] != null
                && ByteMessages.Instance.MeasureModeIncoming[2] != null;
            }
        }

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
                _name = value;
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