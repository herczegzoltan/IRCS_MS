
using IRCS_MS.Infrastructure;
using IRCS_MS.Infrastructure.XmlHandler;
using IRCS_MS.Model;
using System.Collections.Generic;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;


namespace IRCS_MS.ViewModel
{
    public class ServiceModeViewModel : NotifyViewModel
    {

        private List<string> _channelTypes;

        //COMPortManager.DataReceived += new SerialDataReceivedEventHandler(DataRecieved);

        private ICommand _psuOnCommand;
        private ICommand _psuOffCommand;
        private ICommand _resetOnCommand;
        private ICommand _resetOffCommand;
        private ICommand _modulInitCommand;
        private ICommand _changeCommand;
        private ICommand _systemBusWriteCommand;
        private ICommand _systemBusReadCommand;
        private ICommand _fcnGenOnCommand;
        private ICommand _fcnGenOffCommand;
        private ICommand _analGenRunCommand;

        public ICommand PsuOnCommand => _psuOnCommand;
        public ICommand PsuOffCommand => _psuOffCommand;
        public ICommand ResetOnCommand => _resetOnCommand;
        public ICommand ResetOffCommand => _resetOffCommand;
        public ICommand ModulInitCommand => _modulInitCommand;
        public ICommand ChangeCommand => _changeCommand;
        public ICommand SystemBusWriteCommand => _systemBusWriteCommand;
        public ICommand SystemBusReadCommand => _systemBusReadCommand;
        public ICommand FcnGenOnCommand => _fcnGenOnCommand;
        public ICommand FcnGenOffCommand => _fcnGenOffCommand;
        public ICommand AnalGenRunCommand => _analGenRunCommand;

        XmlFilter xmlData = null;


        public ServiceModeViewModel()
        {
            _psuOnCommand = new DelegateCommand(Temp);
            _psuOffCommand = new DelegateCommand(Temp);
            _resetOnCommand = new DelegateCommand(Temp);
            _resetOffCommand = new DelegateCommand(Temp);
            _modulInitCommand = new DelegateCommand(Temp);
            _changeCommand = new DelegateCommand(Temp);
            _systemBusWriteCommand = new DelegateCommand(Temp);
            _systemBusReadCommand = new DelegateCommand(Temp);
            _fcnGenOnCommand = new DelegateCommand(Temp);
            _fcnGenOffCommand = new DelegateCommand(Temp);
            _analGenRunCommand = new DelegateCommand(Temp);

            xmlData = new XmlFilter();
            ChannelTypes = xmlData.ServiceModeGetChannelNames();

        }

        private string _MyProp;

        public void Temp()
        {
            MessageBox.Show(MyProp);
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
                OnPropertyChanged("CardTypes");
            }
        }

        public string MyProp
        {
            get
            {
                return _MyProp;
            }
            set
            {
                _MyProp = value;
                OnPropertyChanged("MyProp");
            }
        }
        #endregion
    }
}
