
using IRCS_MS.Infrastructure;
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


        public SystemBusWriteCommand SystemBusWriteCommand { get; set; }
        public SystemBusReadCommand SystemBusReadCommand { get; set; }
        public PsuOffCommand PsuOffCommand { get; set; }
        public PsuOnCommand PsuOnCommand { get; set; }
        public ResetOffCommand ResetOffCommand { get; set; }
        public ResetOnCommand ResetOnCommand { get; set; }
        public AnalGenRunCommand AnalGenRunCommand { get; set; }
        public ChangeCommand ChangeCommand { get; set; }
        public FcnGenOffCommand FcnGenOffCommand { get; set; }
        public FcnGenOnCommand FcnGenOnCommand { get; set; }
        public ModulInitCommand ModulInitCommand { get; set; }

        XmlFilter xmlData = null;

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
            ChangeCommand = new ChangeCommand(this);
            FcnGenOffCommand = new FcnGenOffCommand(this);
            FcnGenOnCommand = new FcnGenOnCommand(this);
            ModulInitCommand = new ModulInitCommand(this);


            xmlData = new XmlFilter();
            ChannelTypes = xmlData.ServiceModeGetChannelNames();
            FrequencyTypes = xmlData.ServiceModeGetDefaultValuesByTag("frequency");
            AmplitudeTypes = xmlData.ServiceModeGetDefaultValuesByTag("amplitude");

        }


        public void Temp()
        {
            MessageBox.Show("asd");
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
                SubChannelTypes = xmlData.ServiceModeGetSubChannelNames(SelectedChannelType);

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
