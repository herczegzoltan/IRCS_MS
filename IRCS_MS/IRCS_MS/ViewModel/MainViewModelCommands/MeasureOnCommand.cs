using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IRCS_MS.ViewModel.MainViewModelCommands
{
    public class MeasureOnCommand : ICommand
    {

        public MainViewModel VM { get; set; }

        public MeasureOnCommand(MainViewModel vM)
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
            VM.SendMeasureOn();
        }
    }
}
