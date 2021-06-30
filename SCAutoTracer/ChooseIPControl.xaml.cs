using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SCAutoTracer
{
    /// <summary>
    /// Логика взаимодействия для ChooseIPControl.xaml
    /// </summary>
    public partial class ChooseIPControl : UserControl
    {
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items),
            typeof(List<TimeAndIP>), typeof(ChooseIPControl));
        public List<TimeAndIP> Items
        {
            get { return (List<TimeAndIP>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public ChooseIPControl()
        {
            InitializeComponent();
        }

        public ChooseIPControl(List<TimeAndIP> list)
            : this()
        {
            Items = list;
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (combobox.SelectedItem == null)
            {
                nestedContent.Content = null;
                return;
            }

            nestedContent.Content = new RunTraceControl((TimeAndIP)combobox.SelectedItem);
        }
    }
}
