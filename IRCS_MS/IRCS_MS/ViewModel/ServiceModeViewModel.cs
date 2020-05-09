
using IRCS_MS.Infrastructure;
using IRCS_MS.Infrastructure.ServiceMode;
using IRCS_MS.Infrastructure.XmlHandler;
using IRCS_MS.Model;
using IRCS_MS.ViewModel.ServiceModeViewModelCommands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;


namespace IRCS_MS.ViewModel
{
    public class ServiceModeViewModel : INotifyPropertyChanged
    {
        private List<string> _channelTypes;
        private List<string> _subChannelTypes;
        private string _selectedChannelType;
        private List<string> _frequencyTypes;
        private List<string> _amplitudeTypes;
        // COMPort;

        #region Commands

        public SystemBusWriteCommand SystemBusWriteCommand { get; set; }
        public SystemBusReadCommand SystemBusReadCommand { get; set; }
        public PsuOffCommand PsuOffCommand { get; set; }
        public PsuOnCommand PsuOnCommand { get; set; }
        public ResetOffCommand ResetOffCommand { get; set; }
        public ResetOnCommand ResetOnCommand { get; set; }
        public AnalGenRunCommand AnalGenRunCommand { get; set; }
        public AnalGenRunCommand AnalGenOnCommand { get; set; }
        public AnalGenRunCommand AnalGenOffCommand { get; set; }
        public ChangeCommand ChangeCommand { get; set; }
        public FcnGenOffCommand FcnGenOffCommand { get; set; }
        public FcnGenOnCommand FcnGenOnCommand { get; set; }
        public ModulInitCommand ModulInitCommand { get; set; }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ServiceModeViewModel()
        {
            SystemBusWriteCommand = new SystemBusWriteCommand(this);
            SystemBusReadCommand = new SystemBusReadCommand(this);
            PsuOffCommand = new PsuOffCommand(this);
            PsuOnCommand = new PsuOnCommand(this);
            ResetOffCommand = new ResetOffCommand(this);
            ResetOnCommand = new ResetOnCommand(this);
            AnalGenRunCommand = new AnalGenRunCommand(this);
            AnalGenOnCommand = new AnalGenRunCommand(this);
            AnalGenOffCommand = new AnalGenRunCommand(this);
            ChangeCommand = new ChangeCommand(this);
            FcnGenOffCommand = new FcnGenOffCommand(this);
            FcnGenOnCommand = new FcnGenOnCommand(this);
            ModulInitCommand = new ModulInitCommand(this);

            ChannelTypes = XmlFilterServiceMode.Instance.GetChannelNames();
            FrequencyTypes = XmlFilterServiceMode.Instance.GetDefaultValuesByTag("frequency");
            AmplitudeTypes = XmlFilterServiceMode.Instance.GetDefaultValuesByTag("amplitude");
        }

        //TODO: event unsub before opening this UI and subscrube for this and reverse
        //index position

        public void ChangeButtonClicked()
        {
            //ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing,1,ServiceByteMessagesStandardCommands.PSUON);
        }
        
        public void PsuOnButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing,1,ServiceByteMessagesStandardCommands.PSUON);
        }

        public void PsuOffButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing, 1, ServiceByteMessagesStandardCommands.PSUOFF);
        }
        
        public void ResetOnButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing, 1, ServiceByteMessagesStandardCommands.PSUOFF);
        }
        public void ResetOffButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing, 1, ServiceByteMessagesStandardCommands.PSUOFF);
        }
        public void ModulInitButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing, 1, ServiceByteMessagesStandardCommands.MODULINIT);
        }
        public void WriteButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing, 1, ServiceByteMessagesStandardCommands.WRITECMD);
        }
        public void ReadButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing, 1, ServiceByteMessagesStandardCommands.READCMD);
        } 
        
        public void FuncGenOnButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing, 1, ServiceByteMessagesStandardCommands.FUNCGENON);
        }
        public void FuncGenOffButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing, 1, ServiceByteMessagesStandardCommands.FUNCGENOFF);
        }

        public void AnalyserOnButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing, 1, ServiceByteMessagesStandardCommands.ANALYSERON);
        }

        public void AnalyserOffButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing, 1, ServiceByteMessagesStandardCommands.ANALYSEROFF);
        }

        public void AnalGenRunButtonClicked()
        {
            ServiceByteMessagesStandardCommands.SetValueToOutGoingMassage(ByteMessages.Instance.ServiceModeOutgoing, 1, ServiceByteMessagesStandardCommands.ANALYSEROFF);
        }

        #region Properties

        public List<string> ChannelTypes
        {
            get
            {
                return _channelTypes;
            }
            set
            {
                _channelTypes = value;
                OnPropertyChanged("ChannelTypes");
            }
        }

        public List<string> SubChannelTypes
        {
            get
            {
                return _subChannelTypes;
            }
            set
            {
                _subChannelTypes = value;
                OnPropertyChanged("SubChannelTypes");
            }
        }

        public string SelectedChannelType
        {
            get
            {
                return _selectedChannelType;
            }

            set
            {
                _selectedChannelType = value;
                OnPropertyChanged("ChannelTypes");
                SubChannelTypes = XmlFilterServiceMode.Instance.GetSubChannelNames(SelectedChannelType);

            }
        }
        public List<string> FrequencyTypes
        {
            get
            {
                return _frequencyTypes;
            }
            set
            {
                _frequencyTypes = value;
                OnPropertyChanged("FrequencyTypes");
            }
        }
        public List<string> AmplitudeTypes
        {
            get
            {
                return _amplitudeTypes;
            }
            set
            {
                _amplitudeTypes = value;
                OnPropertyChanged("AmplitudeTypes");
            }
        }
        
        #endregion
    }
}
