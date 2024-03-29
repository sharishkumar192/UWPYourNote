﻿using System;
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

namespace UWPYourNote.View
{ 
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            var currentTheme = Window.Current.Content as FrameworkElement;
            //switch (currentTheme.ActualTheme)
            //{
            //    case ElementTheme.Default:
            //        titleBar.BackgroundColor = Colors.White;
            //        titleBar.ForegroundColor = Colors.White; break;
            //    case ElementTheme.Dark:
            //        titleBar.BackgroundColor = Colors.Black;
            //        titleBar.ForegroundColor = Colors.White; break;
            //    case ElementTheme.Light:
            //        titleBar.BackgroundColor = Colors.White;
            //        titleBar.ForegroundColor = Colors.Black; break;
            //}


            MFrame.Navigate(typeof(SignInPage), MFrame);


        }
    }
}
