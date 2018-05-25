using InTheHand.Net.Bluetooth;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static BluetoothHub.MainWindow;

namespace BluetoothHub
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        BluetoothDeviceInfo[] devicesList;
        ObservableCollection<DeviceItem> foundItems = new ObservableCollection<DeviceItem>();
        ObservableCollection<DeviceItem> AddedDevices;

        public AddWindow(ObservableCollection<DeviceItem> collection)
        {
            InitializeComponent();
            AddedDevices = collection;
            Scan();
            lbFoundDevices.ItemsSource = foundItems;
        }

        private void DeviceListButton_Click(object sender, RoutedEventArgs e)
        {
            DeviceItem item = (DeviceItem)(sender as Button).DataContext;
            foundItems.Remove(item);
            AddedDevices.Add(item);
        }

        private void Scan()
        {
            ScanStatus.Text = "Scanning for devices...";           
            //Create a new instance of the BluetoothClient class
            BluetoothClient thisDevice = new BluetoothClient();
            //Create a new instance of BluetoothComponent by passing this device
            BluetoothComponent component= new BluetoothComponent(thisDevice);

            //Add an event listener for when a device is discovered
            component.DiscoverDevicesProgress += BluetoothComponent_DiscoverDevicesProgress;
            //Add an event listener for when the discovery process is finished
            component.DiscoverDevicesComplete += BluetoothComponent_DiscoverDevicesComplete;

            //Start discovery of devices
            component.DiscoverDevicesAsync(8, true, true, true, false, thisDevice);
        }

        //Called everytime a new device is discovered
        void BluetoothComponent_DiscoverDevicesProgress(object sender, DiscoverDevicesEventArgs e)
        {
            //Get the device(s) that was just discovered
            devicesList = e.Devices;
        }

        Object Source(String type)
        {
            if (type.Contains("Audio"))
            {
                return Application.Current.FindResource("HeadphonesImage");
            }
            if (type.Contains("Computer"))
            {
                return Application.Current.FindResource("ComputerImage");
            }
            if (type.Contains("Keyboard"))
            {
                return Application.Current.FindResource("KeyboardImage");
            }
            if (type.Contains("Peripheral"))
            {
                return Application.Current.FindResource("MouseImage");
            }
            return Application.Current.FindResource("DeviceImage");
        }

        //Called after the discovery process is completed
        void BluetoothComponent_DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e)
        {
            ScanStatus.BeginAnimation(OpacityProperty, null);
            if (devicesList == null)
            {
                ScanStatus.Text = "No devices found";
            }
            else
            {
                ScanStatus.Text = "Select a device below to connect";
                foreach (BluetoothDeviceInfo device in devicesList)
                {
                    foundItems.Add(new DeviceItem() { Title = device.DeviceName, Level = 100, Source = Source(device.ClassOfDevice.MajorDevice.ToString()) });
                }
            }
        }
    }
}
