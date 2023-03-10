using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YourNoteUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoteColorOptions : Page, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public NoteColorOptions()
        {
            this.InitializeComponent();
        }

        private void NoteColorClick(object sender, RoutedEventArgs args)
        {
            //Button button = (Button)sender;
            //Button p = this.Parent as Button;
           
            // p.Foreground = (SolidColorBrush)button.Foreground;
        }

        private bool _option1IsSelected = true;

        public bool Option1IsSelected
        {
            get { return _option1IsSelected; }
            set { _option1IsSelected = value;
                OnPropertyChanged();
            }
        }

        private bool _option2IsSelected = false;

        public bool Option2IsSelected
        {
            get { return _option2IsSelected; }
            set { _option2IsSelected = value;
                OnPropertyChanged();
            }
        }


        private bool _option3IsSelected = false;

        public bool Option3IsSelected
        {
            get { return _option3IsSelected; }
            set { _option3IsSelected = value;
                OnPropertyChanged();
            }
        }


        private bool _option4IsSelected = false;

      
        public bool Option4IsSelected
        {
            get { return _option4IsSelected; }
            set { _option4IsSelected = value;
                OnPropertyChanged();
            }
        }




        private void StyleOptionsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
          //  listBox.SelectedItem;

        }
    }
}
