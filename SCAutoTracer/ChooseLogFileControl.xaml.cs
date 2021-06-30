using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для ChooseLogFileControl.xaml
    /// </summary>
    public partial class ChooseLogFileControl : UserControl
    {

        private static readonly Regex regexMatch = new(@"\w*connected\sto\s[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+\|[0-9]+\w*");
        private static readonly Regex regexIP = new(@"\w*[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+\w*");
        private static readonly Regex regexTime = new(@"[0-9]{2}:[0-9]{2}:[0-9]{2}\.[0-9]{3}\w*");

        public ChooseLogFileControl()
        {
            InitializeComponent();

            logPath.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Games\StarConflict\logs";
            logPath.TextChanged += LogPathTextChanged;
        }

        private void ShowException(string message)
        {
            TextBlock exception = new();
            exception.FontSize = 30;
            exception.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            exception.Text = message;
            nestedContent.Content = exception;
        }

        private void LogPathTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!File.Exists(logPath.Text))
            {
                ShowException("Файл не существует");
                return;
            }

            if (!logPath.Text.EndsWith(".log"))
            {
                ShowException("Неподходящее расширение файла");
                return;
            }

            List<TimeAndIP> list = new();
            foreach (var line in File.ReadAllLines(logPath.Text))
            {
                if (!regexMatch.IsMatch(line)) continue;

                Match time = regexTime.Match(line);
                if (!time.Success) continue;

                Match ip = regexIP.Match(line);
                if (!ip.Success) continue;

                list.Add(new TimeAndIP(time.Value, ip.Value));
            }

            nestedContent.Content = new ChooseIPControl(list);
        }

        private void ShowDialog(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new();
            dialog.Filter = "Файлы логов|*.log";
            dialog.InitialDirectory = logPath.Text;
            if (dialog.ShowDialog() == true)
            {
                logPath.Text = dialog.FileName;
            }
        }
    }
}
