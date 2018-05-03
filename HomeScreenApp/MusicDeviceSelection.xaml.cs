using SharpCaster.Models;
using SharpCaster.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HomeScreenApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MusicDeviceSelection : Page
    {
        int chromecastCounter;

        public MusicDeviceSelection()
        {
            this.InitializeComponent();
            this.spinner.IsActive = true;
            this.chromecastCounter = 0;
            loadDevicesAsync();
        }

        public async void loadDevicesAsync()
        {
            this.chromecastCounter = 0;
            List<ChromecastItem> items = new List<ChromecastItem>();

            Debug.WriteLine("Searching for cast devices...");
            ObservableCollection<Chromecast> chromecasts = await ChromecastService.Current.StartLocatingDevices("192.168.0.108");
            Debug.WriteLine("Found " + chromecasts.Count + " elements");


            foreach (Chromecast chromecast in chromecasts) {
                ChromecastItem item = new ChromecastItem()
                {
                    DeviceName = chromecast.FriendlyName,
                    DeviceUri = chromecast.DeviceUri,
                    DeviceId = "dev" + this.chromecastCounter++,
                    Device = chromecast
                };

                items.Add(item);
                Debug.WriteLine("Added cast device " + chromecast.FriendlyName);
                Debug.WriteLine("\tDevice URI: " + chromecast.DeviceUri);
                Debug.WriteLine("\tDevice ID: " + item.DeviceId);
                
            }

            this.deviceList.ItemsSource = items;
            this.spinner.IsActive = false;
        }
        
        private void deviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChromecastItem item = (ChromecastItem)this.deviceList.SelectedItem;
            Debug.WriteLine("Device URI to send: " + item.DeviceUri);
            Debug.WriteLine("\tFriendly name: " + item.DeviceName);

            this.Frame.Navigate(typeof(MediaControlPage), item.DeviceUri);
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }
    }

    public class ChromecastItem
    {
        public string DeviceName { get; set; }
        public Uri DeviceUri { get; set; }
        public string DeviceId { get; set; }
        public Chromecast Device { get; set; }
    }
}
