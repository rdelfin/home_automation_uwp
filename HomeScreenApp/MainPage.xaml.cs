using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HomeScreenApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void menuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Selection changed");
            if (this.menuList.SelectedItem == this.musicMenu)
            {
                Debug.WriteLine("Selection was music.");
                this.Frame.Navigate(typeof(MusicDeviceSelection));
            }
            if (this.menuList.SelectedItem == this.wifiMenu)
            {
                Debug.WriteLine("Selection was wifi.");
                this.Frame.Navigate(typeof(WifiPage));
            }
        }

        private void powerButton_Click(object sender, RoutedEventArgs e)
        {
            ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.FromSeconds(0));
        }

        private async void timerCall(object sender, object e) {
            updateClock();
        }

        private static async Task ExecuteOnUiThread(DispatchedHandler yourAction)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, yourAction);
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            updateClock();
            this.timer = new DispatcherTimer();
            this.timer.Tick += timerCall;
            this.timer.Interval = new TimeSpan(0, 0, 1);
            this.timer.Start();
        }

        private async void updateClock() {
            await ExecuteOnUiThread(() => {
                this.dateText.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                this.timeText.Text = DateTime.Now.ToString("HH:mm:ss");
            });
        }
    }
}
