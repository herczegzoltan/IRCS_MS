using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IRCS_MS.ViewModel.ServiceModeViewModelCommands
{
    public class SystemBusReadCommand : ICommand
    {
        public ServiceModeViewModel VM { get; set; }

        public SystemBusReadCommand(ServiceModeViewModel vM)
        {
            VM = vM;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;

        }

        public void Execute(object parameter)
        {
            VM.ReadButtonClicked();
        }
    }
}
