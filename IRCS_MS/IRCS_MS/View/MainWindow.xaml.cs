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
using MahApps.Metro.Controls;

namespace IRCS_MS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

        }

        private void OpenServiceModWindow(object sender, RoutedEventArgs e)
        {
            ServiceModeWindow smw = new ServiceModeWindow();
            this.Close();
            smw.Show();
        }
    }
}
