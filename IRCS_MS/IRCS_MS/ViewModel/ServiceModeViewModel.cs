
using IRCS_MS.Infrastructure;
using IRCS_MS.Model;
using IRCS_MS.ViewModel.Helper;
using System.Windows.Input;
using MessageBox = System.Windows.Forms.MessageBox;


namespace IRCS_MS.ViewModel
{
    public class ServiceModeViewModel : NotifyViewModel
    {

        private ICommand _connectCommand;
        private ICommand _disConnectCommand;
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

        public ICommand ConnectCommand => _connectCommand;
        public ICommand DisConnectCommand => _disConnectCommand;
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

        public ServiceModeViewModel()
        {
            _connectCommand = new DelegateCommand(Temp);
            _disConnectCommand = new DelegateCommand(Temp);
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
        }

        private string _MyProp;

        public void Temp()
        {
            ConnectToDeviceCommand asd = new ConnectToDeviceCommand(this);

            asd.dd();

            MessageBox.Show(MyProp);
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
    }
}
