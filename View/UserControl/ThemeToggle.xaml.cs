using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public sealed partial class ThemeToggle : UserControl, INotifyPropertyChanged
    {

        public event Action ToggleThemeClick;
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public ThemeToggle()
        {
            this.InitializeComponent();
   
            CheckThemeForToggle();
        }
        public void CheckThemeForToggle()
        {
            ChangeAccentColor.Themes currentTheme = SaveAppSettings.LoadThemePreferences();
            switch (currentTheme)
            {
                case ChangeAccentColor.Themes.Light:
                {
                        ToggleIsChecked = false;            
                        break;
                }

                case ChangeAccentColor.Themes.Dark:
                    {
                        ToggleIsChecked = true;
                        break;
                    }

                case ChangeAccentColor.Themes.System:
                    {
                        ToggleIsChecked = false;
                        if(this.RequestedTheme == ElementTheme.Dark)
                        {
                            ToggleIsChecked = true;
                        }
                            break;
                    }
            }

        }
        private bool _toggleIsChecked;

        public bool ToggleIsChecked
        {
            get { return _toggleIsChecked; }
            set
            {
                _toggleIsChecked = value;
                OnPropertyChanged();
            }
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

        private void UserControl_ActualThemeChanged(FrameworkElement sender, object args)
        {

        }
    }
}
