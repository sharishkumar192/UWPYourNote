using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPYourNoteLibrary.Util;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPYourNote.View.usercontrol
{
    public sealed partial class ThemeToggle : UserControl
    {

        public event Action ToggleThemeClick;
        public ThemeToggle()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentTheme = Window.Current.Content as FrameworkElement;
            ChangeAccentColor.Themes themeToChange = SaveAppSettings.LoadThemePreferences();
            if(themeToChange == ChangeAccentColor.Themes.System)
            {
                if(this.RequestedTheme == ElementTheme.Dark)
                {
                    themeToChange = ChangeAccentColor.Themes.Light;
                }
                else
                {
                    themeToChange = ChangeAccentColor.Themes.Dark;
                }
            }
            else if(themeToChange == ChangeAccentColor.Themes.Light)
            {
                themeToChange = ChangeAccentColor.Themes.Dark;

            }
            else
            {
                themeToChange = ChangeAccentColor.Themes.Light;

            }

            ChangeAccentColor.ChangeTheme(currentTheme, themeToChange);
            ToggleThemeClick?.Invoke();
        }
    }
}
