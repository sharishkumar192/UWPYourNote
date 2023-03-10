using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YourNoteUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.InactiveBackgroundColor = titleBar.BackgroundColor = Windows.UI.Colors.White;
            titleBar.InactiveForegroundColor = titleBar.ForegroundColor = Windows.UI.Colors.Black;

            titleBar.ButtonPressedBackgroundColor = titleBar.ButtonHoverBackgroundColor = titleBar.ButtonInactiveBackgroundColor = titleBar.ButtonBackgroundColor = Windows.UI.Colors.White;
            titleBar.ButtonPressedForegroundColor = titleBar.ButtonHoverForegroundColor = 
                titleBar.ButtonInactiveForegroundColor = titleBar.ButtonForegroundColor = Windows.UI.Colors.Black;
            
            MFrame.Navigate(typeof(SignInPage), MFrame);

        }
    }
}
