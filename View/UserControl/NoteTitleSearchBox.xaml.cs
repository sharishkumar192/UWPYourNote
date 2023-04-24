using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPYourNote.ViewModels;
using UWPYourNoteLibrary.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWPYourNote.ViewModels.usercontrol;
using YourNoteUWP.ViewModels.Contract;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPYourNote.View.usercontrol
{
    public sealed partial class NoteTitleSearchBox : UserControl, INoteTitleSearchBox
    {
        static UWPYourNoteLibrary.Models.Note selectedNoteFromDisplay = null;
        public UWPYourNoteLibrary.Models.Note _selectedNote = null;
        public event Action<Note> DisplaySelectedNote;
        public NoteTitleSearchVM noteTitleSearchVM;

        public event Action UserControlLostFocus;

    

        public NoteTitleSearchBox()
        {
            this.InitializeComponent();
            
            noteTitleSearchVM = NoteTitleSearchVM.Singleton;
            noteTitleSearchVM.noteTitleSearchView = this;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public User LoggedUser
        {
            get 
            { 
                var user = (User)GetValue(LoggedUserProperty); 
                return user;
            }
            set
            {
                SetValue(LoggedUserProperty, value);

            }
        }

        // Using a DependencyProperty as the backing store for LoggedUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoggedUserProperty =
            DependencyProperty.Register("LoggedUser", typeof(User), typeof(NoteTitleSearchBox), new PropertyMetadata(null));




        bool ChangeVar()
        {
            if (_selectedNote != null)
            {
                _selectedNote = null;
                return false;
            }
            return true;
        }

        public void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //    throw new Exception();
                //    throw new Exception();
                if (ChangeVar())
                {
                   // SearchPopupIsOpen = true;
                    TextBox contentOfTextBox = (TextBox)sender;
                    var lowerText = contentOfTextBox.Text.ToLower();
                    var user = LoggedUser.userId;
                    if (user != null)
                    {
                        noteTitleSearchVM.GetSuggestedAndRecentNotes(user, lowerText);
                    }
                    
                }
            }
            catch (Exception m)
            {
                Logger.WriteLog(m.Message);
            }
        }

        public void SuggestionContainerItemClick(object sender, ItemClickEventArgs e)
        {
            selectedNoteFromDisplay = (Note)e.ClickedItem;
            selectedNoteFromDisplay.searchCount++;
           // SearchPopupIsOpen = false;
           SearchPopup.IsOpen = false;
            Suggestion.ItemsSource = null;
            SearchTextBoxText = null;
            DisplaySelectedNote?.Invoke(selectedNoteFromDisplay);
            

        }


        //----------------------------Search Text Box---------------------------------------------------




        private string _searchTextBoxText;
        public string SearchTextBoxText
        {
            get { return _searchTextBoxText; }
            set
            {
                _searchTextBoxText = value;

                OnPropertyChanged();
            }
        }

        private void NoteTitleSearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UserControlLostFocus?.Invoke();
        }


        //----------------------------Search Popup---------------------------------------------------


        //private bool _searchPopupIsOpen = false;
        //public bool SearchPopupIsOpen
        //{
        //    get { return _searchPopupIsOpen; }
        //    set
        //    {
        //        _searchPopupIsOpen = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private Visibility _suggestionContentVisibility = Visibility.Visible;
        //public Visibility SuggestionContentVisibility
        //{
        //    get { return _suggestionContentVisibility; }
        //    set
        //    {
        //        _suggestionContentVisibility = value;
        //        OnPropertyChanged();
        //    }
        //}



    }
}
