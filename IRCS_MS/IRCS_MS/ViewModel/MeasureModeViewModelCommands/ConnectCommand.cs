using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IRCS_MS.ViewModel.MainViewModelCommands
{
    public class ConnectCommand : ICommand
    {
        public MeasureModeViewModel VM { get; set; }

        public ConnectCommand(MeasureModeViewModel vM)
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
            VM.ConnectToDevice();
        }
    }
}
