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
        private static readonly Regex regexFullTime = new(@"[0-9]{2}:[0-9]{2}:[0-9]{2}\.[0-9]{3}\w*");
        private static readonly Regex regexShortTime = new(@"[0-9]{2}:[0-9]{2}\w*");

        private static readonly Regex regexExceptions= new(@"\w*DR_NETWORK_FAIL\w*|\w*NETWORK_FAIL\w*|\w*CLIENT_COULD_NOT_CONNECT\w*");

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

            List<TimeAndIP> timeList = new();

            FileStream stream = new(logPath.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader reader=new(stream);
            string[] lines = reader.ReadToEnd().Split("\r\n".ToCharArray());

            for (int i=0;i<lines.Length; i++)
            {
                string line = lines[i];

                if (!regexExceptions.IsMatch(line)) continue;

                Match time = regexShortTime.Match(line);
                if (time.Success && !timeList.Any(o => o.Time.Equals(time.Value)))
                {
                    Match ip= regexIP.Match( lines.Take(i + 1).Reverse().First(str => regexMatch.IsMatch(str)));
                    if(ip.Success)
                    {
                        timeList.Add(new(time.Value,ip.Value));
                    }
                }
            }

            nestedContent.Content = new ChooseIPControl(timeList);
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
