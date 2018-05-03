using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    }
}
