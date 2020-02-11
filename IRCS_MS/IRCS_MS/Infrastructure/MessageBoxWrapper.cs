using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace IRCS_MS.Infrastructure
{
    public class MessageBoxWrapper
    {
        public static MessageBoxResult Show(string msg, string title, MessageBoxButton buttonStyle, MessageBoxImage image)
        {
            
            var result = MessageBoxResult.None;

            if (Application.Current.Dispatcher.CheckAccess())
                result = MessageBox.Show(Application.Current.MainWindow, msg, title, buttonStyle, image);
            else
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => {
                    result = MessageBox.Show(Application.Current.MainWindow, msg, title, buttonStyle, image);
                }));

            return result;
        }
    }
}
