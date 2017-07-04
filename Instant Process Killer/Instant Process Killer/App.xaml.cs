using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Instant_Process_Killer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string fileName = "InstantProcessKillerSaveFile.dat";
        public static ObservableCollection<string> _processList;
        public static int _id = new int();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _processList = SaveLoad.readBinary<ObservableCollection<string>>(fileName);

            if (_processList == null)
            {
                _processList = new ObservableCollection<string>();
            }
            _id = 0;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            //MainWindow.MyNotifyIcon.Dispose();

            SaveLoad.writeBinary("InstantProcessKillerSaveFile.dat", _processList);
            Thread.Sleep(1000);
        }
    }
}
