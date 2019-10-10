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
using SerialCommunicator.Resource;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

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

        private ICommand _reportSave;
        private ICommand _asd;


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

        public ICommand CmdReportSave => _reportSave;



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



        XmlFilter xmlData = null;

        SerialPort COMPort = null;

        private DateTime _currentDateTime;

        private bool WasItRun = false;

        private string[] saveable = new string[] { };
        List<string> myCollection = new List<string>();

        private int countBytes = 0;

        private int _schauerNumber;

        Measurement measurement = null;
        private string _currentMeasureCount = "Measured data to save: 0";
        private bool _reportFieldState;
        private bool _reportCheckBoxEnabled;

        #endregion

        public MainViewModel()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            _connectCommand = new DelegateCommand(ConnectToDevice);
            _disConnectCommand = new DelegateCommand(DisConnect);
            _measureOn = new DelegateCommand(SendMeasureOn);
            _measureOff = new DelegateCommand(SendMeasureOff);
            _run = new DelegateCommand(SendRun);
            _reportSave = new DelegateCommand(SaveReport);


            AvailablePorts = SerialCommunicationSettings.ListOfSerialPorts();
            BaudRates = SerialCommunicationSettings.ListOfSerialBaudRates();

            xmlData = new XmlFilter();
            CardTypes = xmlData.GetCardTypeNames();

            UpdateTimeUI();

            UIElementUpdater(UIElementStateVariations.ConnectBeforeClick);
            ReadingSerialState();


            measurement = new Measurement();

        }

        private void MeasureTypeComboBoxChanged()
        {
            if (SelectedMeasureType == "AutoMeasure")
            {
                ReportFieldState = true;
                ReportCheckBoxEnabled = true;
            }
            else
            {
                ReportFieldState = false;
                ReportCheckBoxEnabled = false;
            }
        }

        private enum UIElementStateVariations { ConnectBeforeClick, ConnectAfterClick, DisConnectClick, DisConnectBase, CardAndMeasureSelected, MeasureOffClick, MeasureOnAfterClick }

        private void UIElementUpdater(UIElementStateVariations uev)
        {
            switch (uev)
            {
                case UIElementStateVariations.ConnectBeforeClick:
                    UIElementUpdaterHelper(true, false, false, false, false, false, false);
                    break;
                case UIElementStateVariations.ConnectAfterClick:
                    UIElementUpdaterHelper(false, true, false, false, false, true, false);
                    break;
                case UIElementStateVariations.DisConnectBase:
                    UIElementUpdaterHelper(false, true, false, false, false, false, false);
                    break;
                case UIElementStateVariations.DisConnectClick:
                    UIElementUpdaterHelper(true, false, false, false, false, false, false);
                    break;
                case UIElementStateVariations.CardAndMeasureSelected:
                    UIElementUpdaterHelper(false, true, true, false, false, true, false);
                    break;
                case UIElementStateVariations.MeasureOffClick:
                    UIElementUpdaterHelper(false, true, true, false, false, true, false);
                    break;
                case UIElementStateVariations.MeasureOnAfterClick:
                    UIElementUpdaterHelper(false, true, false, true, true, false, false);
                    break;
                default:
                    break;
            }
        }
        private void UIElementUpdaterHelper(
            bool connectButton, bool disconnectButton,
            bool measureOnButton, bool measureOffButton,
            bool runButton, bool cardAndMeasureType, bool reportField)
        {
            CmdConnectIsEnabled = connectButton;
            CmdDisConnectIsEnabled = disconnectButton;
            CmdMeasureOnIsEnabled = measureOnButton;
            CmdMeasureOffIsEnabled = measureOffButton;
            CmdRunIsEnabled = runButton;
            CmdCardTypeIsEnabled = cardAndMeasureType;
            CmdMeasureTypeIsEnabled = cardAndMeasureType;
            //ReportFieldState = reportField;

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
                    //COMPort.DataReceived += new SerialDataReceivedEventHandler(DataRecieved);

                    COMPort.Open();
                    UIElementUpdater(UIElementStateVariations.ConnectAfterClick);
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
            //Thread _thread = null;
            var taskState = Task.Run(() =>
            {
                //_thread = Thread.CurrentThread;
                while (true)
                {
                    Thread.Sleep(500);

                    if (_runningTask)
                    {
                        StateOfDevice = "State: " + (COMPort.IsOpen ? "Connected!" : "Not connected!");
                        StateOfDeviceColor = (COMPort.IsOpen ? "Green" : "Red");
                    }

                }
            });
        }

        private void DisConnect()
        {
            //_runningTask = false;

            //ReadingSerialState();
            DisConfigureDevice();
            UIElementUpdater(UIElementStateVariations.DisConnectClick);
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
            UIElementUpdater(UIElementStateVariations.MeasureOnAfterClick);
        }

        private void SendMeasureOff()
        {
            ByteMessageBuilder.SetByteArray(0, xmlData.GetMeasureOff());
            ByteMessageBuilder.SetByteArray(1, 0x00);
            ByteMessageBuilder.SetByteArray(2, 0x00);
            ByteMessageBuilder.SetByteArray(3, 0x00);
            ByteMessageBuilder.SetByteArray(4, xmlData.GetEOF());

            LoopMessagesArrayToSend();
            UIElementUpdater(UIElementStateVariations.MeasureOffClick);
        }

        private void SendRun()
        {
            ByteMessageBuilder.SetByteArray(0, xmlData.GetMeasureOn());
            ByteMessageBuilder.SetByteArray(1, xmlData.GetSelectedCardTypeValue(SelectedCardType));
            ByteMessageBuilder.SetByteArray(2, xmlData.GetSelectedMeasurementValue(SelectedCardType, SelectedMeasureType));
            ByteMessageBuilder.SetByteArray(3, xmlData.GetRun());
            ByteMessageBuilder.SetByteArray(4, xmlData.GetEOF());

            WasItRun = true;
            LoopMessagesArrayToSend();
        }

        private void LoopMessagesArrayToSend()
        {
            ByteMessageBuilder.SetByteIncomingArray(0, String.Empty);
            ByteMessageBuilder.SetByteIncomingArray(1, String.Empty);
            ByteMessageBuilder.SetByteIncomingArray(2, String.Empty);

            for (int i = 0; i < ByteMessageBuilder.GetByteArray().Length; i++)
            {
                SendData(ByteMessageBuilder.GetByteArray()[i]);
            }
        }

        private void DisConfigureDevice()
        {

            ByteMessageBuilder.SetByteArray(0, 0x02);
            ByteMessageBuilder.SetByteArray(1, 0x00);
            ByteMessageBuilder.SetByteArray(2, 0x00);
            ByteMessageBuilder.SetByteArray(3, 0x00);
            ByteMessageBuilder.SetByteArray(4, 0x0D);

            LoopMessagesArrayToSend();
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
                COMPort.DataReceived += new SerialDataReceivedEventHandler(DataRecieved);
                COMPort.Write(dataArray, 0, 1);
            }
        }


        private void DataRecieved(object sender, SerialDataReceivedEventArgs e)
        {
            if (COMPort.IsOpen)
            {
                string incomingByte = COMPort.ReadByte().ToString();

                //MessageRecievedText += countBytes.ToString() + " :" + incomingByte + "\n";
                ByteMessageBuilder.SetByteIncomingArray(countBytes, incomingByte); //34 0 13

                if (countBytes == 2)
                {
                    if (WasItRun)
                    {
                        MessageRecievedText = "Info: " + DateTime.Now.ToString("HH:mm:ss").ToString() + "-> " + 
                                               xmlData.GetSelectedCardTypeName
                                               (ByteMessageBuilder.ConvertDecimalStringToHexString(ByteMessageBuilder.GetByteIncomingArray()[0].ToString())) 
                                               + " -> " +
                                               xmlData.GetResponseData
                                               (ByteMessageBuilder.ConvertDecimalStringToHexString(ByteMessageBuilder.GetByteIncomingArray()[1].ToString()))
                                               + "\n" + MessageRecievedText +  "\n";

                        string result = xmlData.GetResponseData
                                               (ByteMessageBuilder.ConvertDecimalStringToHexString(ByteMessageBuilder.GetByteIncomingArray()[1].ToString()));

                        measurement.AddSchauerNumber((SchauerNumber++).ToString());
                        measurement.AddResultOfMeasurement(result);
                        measurement.AddMeasureType(SelectedMeasureType);
                        CurrentMeasureCount = "Measured data to save: " +measurement.MeasureType.Count().ToString();
                        /*-
                        if (SelectedMeasureType == "AutoMeasure")
                        {
                            WasItRun = true;
                        }
                        else
                        {
                            WasItRun = false;
                        }
                        */
                        WasItRun = false;

                    }
                    else
                    {
                        MessageRecievedText = "Info: " + DateTime.Now.ToString("HH:mm:ss").ToString() + "-> " +
                                               xmlData.GetResponseTranslate
                                               (ByteMessageBuilder.GetByteIncomingArray()[0].ToString(),
                                               ByteMessageBuilder.GetByteIncomingArray()[1].ToString(),
                                               ByteMessageBuilder.GetByteIncomingArray()[2].ToString())
                                               + "\n" + MessageRecievedText + "\n";
                        //MessageRecievedText = "Info: " + DateTime.Now.ToString("HH:mm:ss").ToString() + "-> " +
                        //                    xmlData.GetSelectedCardTypeName
                        //                    (ByteMessageBuilder.ConvertDecimalStringToHexString(ByteMessageBuilder.GetByteIncomingArray()[0].ToString()))
                        //                    + " -> " +
                        //                    xmlData.GetResponseData
                        //                    (ByteMessageBuilder.ConvertDecimalStringToHexString(ByteMessageBuilder.GetByteIncomingArray()[1].ToString()))
                        //                    + "\n" + MessageRecievedText + "\n";

                    }

                    countBytes = 0;

                    //disconnecting
                    WasItDisconnect();
                    ByteMessageBuilder.ResetByteIncomingArray();

                }
                else
                {
                    countBytes++;
                }
            }
        }
        //temp solution values shall be from xml
        private void WasItDisconnect()
        {

            if(xmlData.GetResponseTranslate
                                               (ByteMessageBuilder.GetByteIncomingArray()[0].ToString(),
                                               ByteMessageBuilder.GetByteIncomingArray()[1].ToString(),
                                               ByteMessageBuilder.GetByteIncomingArray()[2].ToString()) == "Disconnected")

            {
                COMPort.Close();
                COMPort.Dispose();
            }
            //if (ByteMessageBuilder.GetByteIncomingArray()[2].ToString() == ByteMessageBuilder.ConvertStringToByte(xmlData.GetEOF()).ToString() //"13"
            //                                && ByteMessageBuilder.GetByteIncomingArray()[1].ToString() == "113"
            //                                && ByteMessageBuilder.GetByteIncomingArray()[0].ToString() == "34") //disconnected

        }


        private void SaveReport()
        {
            if (measurement.MeasureType.Any())
            {
                string FolderPath = FolderDialog();
                if (FolderPath != "")
                {
                    string FileName = $"IRCS_{SelectedCardType}_{measurement.SchauerNumber.First()}_"+
                      $"{int.Parse(measurement.SchauerNumber.Last()) - int.Parse(measurement.SchauerNumber.First()) + 1}";

                    //"IRCS_"CardName"_"kezdőszám"_"hány darab kártya lett mérve".xls;
                    ReportDataHelper.InitializeMeasure(FileName, FolderPath);
                    //ReportDataHelper.SetDataForReport(measurement);
                    ReportDataHelper.PassListTOReport(ReportDataHelper.CreateDataMap(measurement));
                    
                    ReportDataHelper.CreateReportFile();

                    MessageBox.Show("File Saved!");
                }
            }
            else
            {
                MessageBox.Show("No measurement data!");
            }
          

        }
    
        private string FolderDialog()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                return fbd.SelectedPath;
            }
            return "";
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
                MeasureTypeComboBoxChanged();
                if (value != null)
                {
                    UIElementUpdater(UIElementStateVariations.CardAndMeasureSelected);
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

        public int SchauerNumber
        {
            get
            {
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
        

        #endregion
    }
}