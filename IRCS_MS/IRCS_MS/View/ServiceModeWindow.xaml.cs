using IRCS_MS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IRCS_MS.View
{
    /// <summary>
    /// Interaction logic for MainWindowTest.xaml
    /// </summary>
    public partial class ServiceModeWindow
    {
        public ServiceModeWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void OpenMainWindow(object sender, RoutedEventArgs e)
        {

            var mainViewModel = new MainViewModel();
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();
            this.Close();
        }
    }
}
