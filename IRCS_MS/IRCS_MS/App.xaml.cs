using IRCS_MS.View;
using IRCS_MS.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IRCS_MS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //var smainViewModel = new ServiceModeViewModel();
            //var smainWindow = new ServiceModeWindow { DataContext = smainViewModel };
            //smainWindow.Close();

            var mainViewModel = new MeasureModeViewModel();
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainWindow.Show();


          


            // mainWindow.Show();
        }
    }
}
