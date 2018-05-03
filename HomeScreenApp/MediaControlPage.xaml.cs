using SharpCaster.Controllers;
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
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class MediaControlPage : Page
    {
        private YouTubeController controller;
        private Chromecast chromecast;
        bool applicationStarted = false;

        public MediaControlPage()
        {
            this.applicationStarted = false;
            this.InitializeComponent();
        }

        private void disableAll() {
            this.volumeSlider.IsEnabled = false;
            this.playButton.IsEnabled = false;
            this.prevButton.IsEnabled = false;
            this.nextButton.IsEnabled = false;
            this.spinner.IsActive = true;
        }

        private void enableAll() {
            this.volumeSlider.IsEnabled = true;
            this.playButton.IsEnabled = true;
            this.prevButton.IsEnabled = true;
            this.nextButton.IsEnabled = true;
            this.spinner.IsActive = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.disableAll();
            Uri deviceUri = e.Parameter as Uri;
            if (deviceUri != null) {
                Debug.WriteLine("Device URI: " + deviceUri.ToString());
                loadChromecast(deviceUri);
            }
            else
                Debug.WriteLine("Device URI is null!");
        }

        private async void loadChromecast(Uri uri) {
            ObservableCollection<Chromecast> chromecasts = await ChromecastService.Current.StartLocatingDevices("192.168.0.108");
            this.chromecast = chromecasts.First((Chromecast c) => { return c.DeviceUri == uri; });
            if (this.chromecast == null) {
                Debug.WriteLine("There was an error connecting to the chromecast.");
                return;
            }

            ChromecastService.Current.ChromeCastClient.ConnectedChanged += async delegate { if (this.controller == null) this.controller = await ChromecastService.Current.ChromeCastClient.LaunchYouTube(); };
            ChromecastService.Current.ChromeCastClient.ApplicationStarted +=
            async delegate {
                while (this.controller == null)
                {
                    await Task.Delay(200);
                }

                await ExecuteOnUiThread(() => {
                    this.enableAll();
                    this.volumeSlider.Value = ChromecastService.Current.ChromeCastClient.Volume.level * 100.0;
                });
                this.applicationStarted = true;
            };
            ChromecastService.Current.ConnectToChromecast(chromecast);
        }

        private void Volume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (this.applicationStarted && this.controller != null) {
                this.controller.SetVolume(this.volumeSlider.Value / 100.0);
            }
        }

        private static async Task ExecuteOnUiThread(DispatchedHandler yourAction)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, yourAction);
        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.controller == null)
                return;
            this.controller.Previous();
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.controller == null)
                return;
            
            if (ChromecastService.Current.ChromeCastClient.MediaStatus.PlayerState == SharpCaster.Models.MediaStatus.PlayerState.Playing)
                this.controller.Pause();
            else
                this.controller.Play();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.controller == null)
                return;
            this.controller.Next();
        }
    }
}
