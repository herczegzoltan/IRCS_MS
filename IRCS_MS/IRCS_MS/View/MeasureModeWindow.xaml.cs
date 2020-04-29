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
using System.Windows.Navigation;
using System.Windows.Shapes;
using IRCS_MS.View;
using IRCS_MS.ViewModel;
using MahApps.Metro.Controls;

namespace IRCS_MS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MeasureModeWindow
    {
        public MeasureModeWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void OpenServiceModWindow(object sender, RoutedEventArgs e)
        {
           

           // ServiceModeWindow smw = new ServiceModeWindow();

            var smainViewModel = new ServiceModeViewModel();
            var smainWindow = new ServiceModeWindow { DataContext = smainViewModel };
            smainWindow.Show();
            this.Close();
            //this.Close();
            //smw.Show();
        }
    }
}
