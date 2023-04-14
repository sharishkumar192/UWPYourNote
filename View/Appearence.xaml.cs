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
using static UWPYourNoteLibrary.Util.ChangeAccentColor;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YourNoteUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Appearence : Page, INotifyPropertyChanged
    {
        public delegate void ThemeChangedEventHandler(ChangeAccentColor.Themes newData);
        ThemePublisher themePublisher;
        public Appearence()
        {
            this.InitializeComponent();
            themePublisher = ThemePublisher.Tp;
            

           // themePublisher.ThemeUpdated += themeSubscriber.OnThemeChangedEventHandler;
            themePublisher.ThemeUpdated += OnThemeChangedEventHandler;

        }
        bool themeCheck;
        bool accentCheck;
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    
 
        public void ChooseAccentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox box = (ListBox)sender;
            ListBoxItem item = box.SelectedItem as ListBoxItem;

            var currentTheme = Window.Current.Content as FrameworkElement;
       
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            if (ChooseAccentSelectedIndex == 0 && accentCheck == false)
            {
                ChooseAccentSelectedIndex = GetPrefferencedAccentColor();
                accentCheck = true;
            }
            ChangeAccentColor.ColorType accentToChange = (ChangeAccentColor.ColorType)ChooseAccentSelectedIndex;

            if (DefaultIsSelected == true)
            {
                LavendarIsSelected =
                ForestIsSelected =
                NighttimeIsSelected = false;
            }
            else if (LavendarIsSelected == true)
            {
                DefaultIsSelected =
                ForestIsSelected =
                NighttimeIsSelected = false;
            }
            else if (ForestIsSelected == true)
            {
                DefaultIsSelected =
                LavendarIsSelected =
                NighttimeIsSelected = false;
            }
            else
            {
                DefaultIsSelected =
                LavendarIsSelected =
                ForestIsSelected = false;
            }

            ChangeAccentColor.ChangeAccent(accentToChange);

        }

        public void ChooseThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            var currentTheme = Window.Current.Content as FrameworkElement;
           
            if (ChooseThemeSelectedIndex == 0 && themeCheck == false)
            {
                ChooseThemeSelectedIndex = GetPrefferencedTheme();
                themeCheck = true;
            }
            ChangeAccentColor.Themes themeToChange = (ChangeAccentColor.Themes)ChooseThemeSelectedIndex;
            if (LightIsSelected == true)
            {
                DarkIsSelected = SystemIsSelected = false;
               
            }
            else if (DarkIsSelected == true)
            {
                SystemIsSelected = LightIsSelected = false;
               
            }
            else
            {
                DarkIsSelected = LightIsSelected = false;
             
            }


            ChangeAccentColor.ChangeTheme(currentTheme, themeToChange);
            themePublisher.UpdateTheme(themeToChange);


            //    ChangeAccentColor.ChangeTheme(windowTheme, themeToChange);

        }


        private int GetPrefferencedAccentColor()
        {
            var currentAccentColor = SaveAppSettings.LoadAccentColorPreferences();

            switch (currentAccentColor)
            {
                case ChangeAccentColor.ColorType.Default: DefaultIsSelected = true; break;
                case ChangeAccentColor.ColorType.Lavendar: LavendarIsSelected = true; break;
                case ChangeAccentColor.ColorType.Forest: ForestIsSelected = true; break;
                case ChangeAccentColor.ColorType.Nighttime: NighttimeIsSelected = true; break;
                default: break;
            }

            return (int)currentAccentColor;
        }

        private int GetPrefferencedTheme()
        {
            var currentTheme = SaveAppSettings.LoadThemePreferences();
            switch (currentTheme)
            {
                case ChangeAccentColor.Themes.System: SystemIsSelected = true; break;
                case ChangeAccentColor.Themes.Dark: DarkIsSelected = true; break;
                case ChangeAccentColor.Themes.Light:
                    LightIsSelected = true;
                    break;
                default: break;

            }
            return (int)currentTheme;
        }
        private int _chooseAccentSelectedIndex;

        public int ChooseAccentSelectedIndex
        {
            get
            {
                return _chooseAccentSelectedIndex;
            }
            set
            {
                _chooseAccentSelectedIndex = value;
                OnPropertyChanged();
            }
        }


        private int _chooseThemeSelectedIndex;

        public int ChooseThemeSelectedIndex
        {
            get
            {
                return _chooseThemeSelectedIndex;

            }
            set
            {
                _chooseThemeSelectedIndex = value;
                OnPropertyChanged();
            }
        }


        private bool _lightIsSelected = false;
        public bool LightIsSelected
        {
            get { return _lightIsSelected; }
            set
            {
                _lightIsSelected = value;
                OnPropertyChanged();

            }
        }





        private bool _darkIsSelected = false;
        public bool DarkIsSelected
        {
            get { return _darkIsSelected; }
            set
            {
                _darkIsSelected = value;
                OnPropertyChanged();
            }
        }




        private bool _systemIsSelected = false;
        public bool SystemIsSelected
        {
            get { return _systemIsSelected; }
            set
            {
                _systemIsSelected = value;
                OnPropertyChanged();
            }
        }

        private bool _defaultIsSelected = false;
        public bool DefaultIsSelected
        {
            get { return _defaultIsSelected; }
            set
            {
                _defaultIsSelected = value;
                OnPropertyChanged();

            }
        }

        private bool _lavendarIsSelected = false;
        public bool LavendarIsSelected
        {
            get { return _lavendarIsSelected; }
            set
            {
                _lavendarIsSelected = value;
                OnPropertyChanged();
            }
        }

        private bool _forestIsSelected = false;
        public bool ForestIsSelected
        {
            get { return _forestIsSelected; }
            set
            {
                _forestIsSelected = value;
                OnPropertyChanged();
            }
        }



        private bool _nighttimeIsSelected = false;
        public bool NighttimeIsSelected
        {
            get { return _nighttimeIsSelected; }
            set
            {
                _nighttimeIsSelected = value;
                OnPropertyChanged();
            }
        }


        public void OnThemeChangedEventHandler(ChangeAccentColor.Themes themeToChange)
        {
            //ChangeAccentColor.ChangeThemeNewView(this.RequestedTheme, themeToChange);
            switch (themeToChange)
            {
                case Themes.System:
                    {
                        this.RequestedTheme = ElementTheme.Default;
                        break;
                    }
                case Themes.Dark:
                    {
                        this.RequestedTheme = ElementTheme.Dark;
                        break;

                    }

                case Themes.Light:
                    {
                        this.RequestedTheme = ElementTheme.Light;
                        break;

                    }
                default:
                    {
                        break;
                    }



            }
    }





    }
}
