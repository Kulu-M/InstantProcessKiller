using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Interop;
using Application = System.Windows.Forms.Application;
using Binding = System.Windows.Data.Binding;
using ListBox = System.Windows.Controls.ListBox;
using MessageBox = System.Windows.MessageBox;

namespace Instant_Process_Killer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            updateProcessesListBox();

            tb_status.Text = "Deactivated";
            tb_status.Foreground = new SolidColorBrush(Colors.Red);
        }

        void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            try
            {
                if ((bool)cb_switchTo.IsChecked)
                {
                    Process[] processArrayToSwitchTo = getCertainProcess(tb_switchTo.Text);

                    foreach (var proc in processArrayToSwitchTo)
                    {
                        BringToFront(proc);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            if ((bool) cb_kill.IsChecked)
            {
                killProcesses();
            }
            else
            {
                minimizeProcesses();
            }

            
        }

        private void minimizeProcesses()
        {
            var localProcesses = Process.GetProcesses();
            try
            {
                foreach (var process in App._processList)
                {
                    foreach (var proc in localProcesses)
                    {
                        if (proc.ProcessName == process)
                        {
                            //minimize Process
                            Console.WriteLine("Process minimized:" + proc.ProcessName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void killProcesses()
        {
            var localProcesses = Process.GetProcesses();
            try
            {
                foreach (var process in App._processList)
                {
                    foreach (var proc in localProcesses)
                    {
                        if (proc.ProcessName == process)
                        {
                            proc.Kill();
                            Console.WriteLine("Process killed:" + proc.ProcessName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private Process[] getCertainProcess(string processName)
        {
            var localProcesses = Process.GetProcessesByName(processName);
            return localProcesses;
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        private void BringToFront(Process pTemp)
        {
            SetForegroundWindow(pTemp.MainWindowHandle);
        }


        private void b_add_Click(object sender, RoutedEventArgs e)
        {
            try
            {   //TODO Remove .exe
                App._processList.Add(tb_process.Text);
                updateProcessesListBox();
                tb_process.Text = "";
            }
            catch
            {
                // ignored
            }
        }

        private void updateProcessesListBox()
        {
            lb_listBoxProcesses.DataContext = App._processList;
            var binding = new Binding();
            lb_listBoxProcesses.SetBinding(ListBox.ItemsSourceProperty, binding);
        }

        private void b_remove_Click(object sender, RoutedEventArgs e)
        {
            if (lb_listBoxProcesses.SelectedItem != null)
            {
                App._processList.Remove(lb_listBoxProcesses.SelectedItem as string);
            }
        }

        private void b_active_Click(object sender, RoutedEventArgs e)
        {
            App._id = HotKeyManager.RegisterHotKey(Keys.Enter, ModifierKeys.None);
            HotKeyManager.HotKeyPressed += HotKeyManager_HotKeyPressed;

            tb_status.Text = "Scanning for input...";
            tb_status.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void b_deactive_Click(object sender, RoutedEventArgs e)
        {
            if (App._id != 0)
            {
                HotKeyManager.UnregisterHotKey(App._id);

                tb_status.Text = "Deactivated";
                tb_status.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void cb_kill_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool) cb_kill.IsChecked && tb_killOrMinimize != null)
            {
                tb_killOrMinimize.Text = "Processes will be killed";
            }
        }

        private void cb_kill_Unchecked(object sender, RoutedEventArgs e)
        {
            if ((bool)cb_kill.IsChecked == false && tb_killOrMinimize != null)
            {
                tb_killOrMinimize.Text = "Processes will be minimized";
            }
        }

        //private void MyNotifyIcon_TrayContextMenuOpen(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    OpenEventCounter.Text = (int.Parse(OpenEventCounter.Text) + 1).ToString();
        //}

        //private void MyNotifyIcon_PreviewTrayContextMenuOpen(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    //marking the event as handled suppresses the context menu
        //    e.Handled = (bool)SuppressContextMenu.IsChecked;

        //    PreviewOpenEventCounter.Text = (int.Parse(PreviewOpenEventCounter.Text) + 1).ToString();
        //}
    }
}
