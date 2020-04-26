﻿
using IRCS_MS.Infrastructure;
using IRCS_MS.Infrastructure.XmlHandler;
using IRCS_MS.Model;
using IRCS_MS.ViewModel.ServiceModeCommands;
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

        //COMPortManager.DataReceived += new SerialDataReceivedEventHandler(DataRecieved);

        //private ICommand _psuOnCommand;
        //private ICommand _psuOffCommand;
        //private ICommand _resetOnCommand;
        //private ICommand _resetOffCommand;
        //private ICommand _modulInitCommand;
        //private ICommand _changeCommand;
        //private ICommand _systemBusWriteCommand;
        //private ICommand _systemBusReadCommand;
        //private ICommand _fcnGenOnCommand;
        //private ICommand _fcnGenOffCommand;
        //private ICommand _analGenRunCommand;

        //public ICommand PsuOnCommand => _psuOnCommand;
        //public ICommand PsuOffCommand => _psuOffCommand;
        //public ICommand ResetOnCommand => _resetOnCommand;
        //public ICommand ResetOffCommand => _resetOffCommand;
        //public ICommand ModulInitCommand => _modulInitCommand;
        //public ICommand ChangeCommand => _changeCommand;
        //public ICommand SystemBusWriteCommand => _systemBusWriteCommand;
        //public ICommand SystemBusReadCommand => _systemBusReadCommand;
        //public ICommand FcnGenOnCommand => _fcnGenOnCommand;
        //public ICommand FcnGenOffCommand => _fcnGenOffCommand;
        //public ICommand AnalGenRunCommand => _analGenRunCommand;

        XmlFilter xmlData = null;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ServiceModeViewModel()
        {
            //_psuOnCommand = new DelegateCommand(Temp);
            //_psuOffCommand = new DelegateCommand(Temp);
            //_resetOnCommand = new DelegateCommand(Temp);
            //_resetOffCommand = new DelegateCommand(Temp);
            //_modulInitCommand = new DelegateCommand(Temp);
            //_changeCommand = new DelegateCommand(Temp);
            //_systemBusWriteCommand = new DelegateCommand(Temp);
            //_systemBusReadCommand = new DelegateCommand(Temp);
            //_fcnGenOnCommand = new DelegateCommand(Temp);
            //_fcnGenOffCommand = new DelegateCommand(Temp);
            //_analGenRunCommand = new DelegateCommand(Temp);
            SystemBusWriteCommand = new SystemBusWriteCommand(this);
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
