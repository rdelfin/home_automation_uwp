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
        public MusicDeviceSelection()
        {
            this.InitializeComponent();
            this.spinner.IsActive = true;
            loadDevicesAsync();
        }

        public async void loadDevicesAsync()
        {
            Debug.WriteLine("Searching for cast devices...");
            ObservableCollection<Chromecast> chromecasts = await ChromecastService.Current.StartLocatingDevices("192.168.0.108");
            Debug.WriteLine("Found " + chromecasts.Count + " elements");

            List<ChromecastItem> sourceList = new List<ChromecastItem>();

            foreach (Chromecast chromecast in chromecasts) {
                sourceList.Add(new ChromecastItem() { DeviceName = chromecast.FriendlyName });
                Debug.WriteLine("Added cast device " + chromecast.FriendlyName);
            }

            this.deviceList.ItemsSource = sourceList;
            this.spinner.IsActive = false;
        }
    }

    public class ChromecastItem
    {
        public string DeviceName { get; set; }
    }
}
