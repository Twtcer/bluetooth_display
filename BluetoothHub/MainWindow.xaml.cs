using InTheHand.Net.Sockets;
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

namespace BluetoothHub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<DeviceItem> items = new ObservableCollection<DeviceItem>();

        public MainWindow()
        {
            InitializeComponent();
            DevicesIC.ItemsSource = items;
        }

        public class DeviceItem
        {
            public string Title { get; set; }
            public int Level { get; set; }
            public Object Source { get; set; }
        }

        private void AddDevice_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(items)
            {
                Owner = Application.Current.MainWindow
            };
            addWindow.ShowDialog();
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            DeviceItem item = (DeviceItem)(sender as Button).DataContext;
            items.Remove(item);
        }


    }
}
