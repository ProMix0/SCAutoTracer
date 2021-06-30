using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Loader;
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

namespace SCAutoTracer
{
    /// <summary>
    /// Логика взаимодействия для RunTraceControl.xaml
    /// </summary>
    public partial class RunTraceControl : UserControl
    {
        private TimeAndIP timeAndIP;

        public RunTraceControl(TimeAndIP timeAndIP)
        {
            this.timeAndIP = timeAndIP;
            InitializeComponent();
        }

        private void RunTrace(object sender, RoutedEventArgs e)
        {
            string tracerPath = Environment.CurrentDirectory + @"\WinMTR.exe";

            if (!File.Exists(tracerPath))
            {
                using Stream stream= File.Create(tracerPath);
                stream.Write(Properties.Resources.WinMTR);
            }

            Process.Start(tracerPath, timeAndIP.IP);
        }
    }
}
