using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWPYourNoteLibrary.Models;
using UWPYourNoteLibrary.Util;
using UWPYourNote.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPYourNote.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page, INotifyPropertyChanged
    {
        private Frame _frame;
        private UWPYourNoteLibrary.Models.Note _selectedNote = null;
        private HomePageViewModel _homePageViewModel;
        static UWPYourNoteLibrary.Models.Note selectedNoteFromDisplay = null;


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public HomePage()
        {
            this.InitializeComponent();
        }


        private delegate void DelUserControlMethod(object sender, object e);

        private void DelegateIntialize()
        {
            DelUserControlMethod delUserControlMethod = new DelUserControlMethod(NoteDisplayPopUpClosed);
            NoteContentPopUp.CallingPageMethod = delUserControlMethod;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            DelegateIntialize();

            NoteEditOptions.NoteDeleteButtonVisibility = NoteEditOptions.NoteShareButtonVisibility = Visibility.Collapsed;
            Tuple<Frame, UWPYourNoteLibrary.Models.User> tuple = (Tuple<Frame, UWPYourNoteLibrary.Models.User>)e.Parameter;
            _frame = tuple.Item1;
            LoggedUser = tuple.Item2;
        }


        private UWPYourNoteLibrary.Models.User _loggedUser;

        public UWPYourNoteLibrary.Models.User LoggedUser
        {
            get { return _loggedUser; }
            set
            {
                _loggedUser = value;
                OnPropertyChanged();
            }

        }

        bool ChangeVar()
        {
            if (_selectedNote != null)
            {
                _selectedNote = null;
                return false;
            }
            return true;
        }


        private async void NoTitle()
        {
            MessageDialog showDialog;

            showDialog = new MessageDialog("To create a note, please give a title!");
            showDialog.Commands.Add(new UICommand("Ok")
            {
                Id = 0
            });
            showDialog.DefaultCommandIndex = 0;
            var result = await showDialog.ShowAsync();
        }



        //----------------------------Main Menu List Box---------------------------------------------------

        private bool _personalNotesIsSelected = true;
        public bool PersonalNotesIsSelected
        {
            get { return _personalNotesIsSelected; }
            set
            {
                _personalNotesIsSelected = value;
                OnPropertyChanged();

            }
        }





        private bool _sharedNotesIsSelected = false;
        public bool SharedNotesIsSelected
        {
            get { return _sharedNotesIsSelected; }
            set
            {
                _sharedNotesIsSelected = value;
                OnPropertyChanged();
            }
        }




        private bool _allNotesIsSelected = false;
        public bool AllNotesIsSelected
        {
            get { return _allNotesIsSelected; }
            set
            {
                _allNotesIsSelected = value;
                OnPropertyChanged();
            }
        }

        private int _mainMenuOptionsSelectedIndex = 0;

        public int MainMenuOptionsSelectedIndex
        {
            get { return _mainMenuOptionsSelectedIndex; }
            set
            {
                _mainMenuOptionsSelectedIndex = value;
                OnPropertyChanged();
            }
        }


        public void MainMenuOptionsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TitleOfNewNoteVisibility = NoteStyleOptionsVisibility = Visibility.Collapsed;
            ListBox box = (ListBox)sender;

            MainMenuOptionsSelectedIndex = box.SelectedIndex;


            if (PersonalNotesIsSelected == true)
            {
                _notesDataItemSource = null;
                SharedNotesIsSelected = AllNotesIsSelected = false;
                TitleText = "My Personal Notes";

                if (_notesDataItemSource == null)
                    _notesDataItemSource = HomePageViewModel.GetPersonalNotes(LoggedUser.userId, true);


                NotesDataItemSource = _notesDataItemSource;

                SearchTextBoxText = "";
                _selectedNote = new Note("", "", "", 0);
                SearchPopupIsOpen = false;

            }
            else if (SharedNotesIsSelected == true)
            {
                _notesDataItemSource = null;
                TitleText = "My Shared Notes";

                PersonalNotesIsSelected = AllNotesIsSelected = false;

                if (_notesDataItemSource == null)
                    _notesDataItemSource = HomePageViewModel.GetSharedNotes(LoggedUser.userId, true);

                NotesDataItemSource = _notesDataItemSource;

                _selectedNote = new Note("", "", "", 0);
                SearchTextBoxText = "";
                SearchPopupIsOpen = false;
            }
            else if (AllNotesIsSelected == true)
            {
                _notesDataItemSource = null;
                TitleText = "All Notes";
                if (_notesDataItemSource == null)
                    _notesDataItemSource = HomePageViewModel.GetAllNotes(LoggedUser.userId, true);
                NotesDataItemSource = _notesDataItemSource;
            }
        }

        //----------------------------Search Text Box---------------------------------------------------
        public void SearchBoxContainerLostFocus()
        {
            SearchPopupIsOpen = false;
        }



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


        public void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //    throw new Exception();
                if (ChangeVar())
                {
                    SearchPopupIsOpen = true;
                    TextBox contentOfTextBox = (TextBox)sender;
                    _homePageViewModel = new HomePageViewModel();
                    if (contentOfTextBox.Text.Length <= 2)
                    {
                        RecentlySearchedVisibility = Visibility.Visible;
                        SuggestionContentVisibility = Visibility.Collapsed;
                        RecentlySearchedItemSource = _homePageViewModel.GetRecentNotes(LoggedUser.userId); 
                    }

                    else
                    {
                       
                        RecentlySearchedVisibility = Visibility.Collapsed;
                        SuggestionContentVisibility = Visibility.Visible;
                        var lowerText = contentOfTextBox.Text.ToLower();
                        SuggestionContentItemSource = _homePageViewModel.GetSuggestedNote(LoggedUser.userId, lowerText);

                    }

                }
            }
            catch (Exception m)
            {
                TextBox contentOfTextBox = (TextBox)sender;
                Logger.WriteLog(m.Message);
            }

        }

        //----------------------------Search Popup---------------------------------------------------

        private bool _searchPopupIsOpen = false;
        public bool SearchPopupIsOpen
        {
            get { return _searchPopupIsOpen; }
            set
            {
                _searchPopupIsOpen = value;

                OnPropertyChanged();
            }
        }



        //----------------------------Search -> Recently Searched List Box ---------------------------------------------------
        private ObservableCollection<UWPYourNoteLibrary.Models.Note> _recentlySearchedItemSource;
        public ObservableCollection<UWPYourNoteLibrary.Models.Note> RecentlySearchedItemSource
        {
            get { return _recentlySearchedItemSource; }
            set
            {
                _recentlySearchedItemSource = value;
                OnPropertyChanged();
            }
        }

        public void RecentlySearchedItemClick(object sender, ItemClickEventArgs e)
        {
            selectedNoteFromDisplay = (Note)e.ClickedItem;
            selectedNoteFromDisplay.searchCount++;
            NoteContentPopUp.DisplayContent(LoggedUser.userId, selectedNoteFromDisplay.noteId, selectedNoteFromDisplay.title, selectedNoteFromDisplay.content, selectedNoteFromDisplay.searchCount, selectedNoteFromDisplay.noteColor, selectedNoteFromDisplay.modifiedDay);
            SearchPopupIsOpen = false;
            NoteDisplayPopUpOpened();


        }



        private Visibility _recentlySearchedVisibility = Visibility.Visible;
        public Visibility RecentlySearchedVisibility
        {
            get { return _recentlySearchedVisibility; }
            set
            {
                _recentlySearchedVisibility = value;
                OnPropertyChanged();
            }
        }










        //----------------------------Search -> Suggestion List View ---------------------------------------------------

        public void SuggestionContainerItemClick(object sender, ItemClickEventArgs e)
        {
            selectedNoteFromDisplay = (Note)e.ClickedItem;
            selectedNoteFromDisplay.searchCount++;
            NoteContentPopUp.DisplayContent(LoggedUser.userId, selectedNoteFromDisplay.noteId, selectedNoteFromDisplay.title, selectedNoteFromDisplay.content, selectedNoteFromDisplay.searchCount, selectedNoteFromDisplay.noteColor, selectedNoteFromDisplay.modifiedDay);
            SearchPopupIsOpen = false;
            NoteDisplayPopUpOpened();


        }


        private ObservableCollection<UWPYourNoteLibrary.Models.Note> _suggestionContentItemSource;
        public ObservableCollection<UWPYourNoteLibrary.Models.Note> SuggestionContentItemSource
        {
            get { return _suggestionContentItemSource; }
            set
            {
                _suggestionContentItemSource = value;
                OnPropertyChanged();

            }
        }







        private Visibility _suggestionContentVisibility = Visibility.Visible;
        public Visibility SuggestionContentVisibility
        {
            get { return _suggestionContentVisibility; }
            set
            {
                _suggestionContentVisibility = value;
                OnPropertyChanged();
            }
        }


        //----------------------------Sign Out Button---------------------------------------------------
        public void LogoutContentTapped()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }

        }

        //--------------------------- Add a New Note------------------------------------------------------------

        private void SaveOrClose(string title, string content)
        {
            if (title != null && content != null && title.Length == 0 && content.Length == 0)
                CreationButtonContent = "Close";
            else
                CreationButtonContent = "Save";

        }

        private string TextChangedFunction(RichEditBox box)
        {
            string text;
            box.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out text);
            Windows.UI.Text.ITextRange range = box.Document.GetRange(0, text.Length - 1);
            if (range.Text == "\r")
                return "";
            return text;
            //return range.Text;
        }


        //----------Title TextBlock

        private string _titleText;

        public string TitleText
        {
            get { return _titleText; }
            set
            {
                _titleText = value;
                OnPropertyChanged();
            }
        }

        //-----------New Note Container

        private Visibility _newNoteVisibility;

        public Visibility NewNoteVisibility
        {
            get { return _newNoteVisibility; }
            set { _newNoteVisibility = value; }
        }

        //--Title---
        private string _titleOfNewNoteText = "";
        public string TitleOfNewNoteText
        {
            get { return _titleOfNewNoteText; }
            set
            {
                _titleOfNewNoteText = value;
                OnPropertyChanged();
            }
        }



        private Visibility _titleOfNewNoteVisibility = Visibility.Collapsed;
        public Visibility TitleOfNewNoteVisibility
        {
            get { return _titleOfNewNoteVisibility; }
            set
            {
                _titleOfNewNoteVisibility = value;
                OnPropertyChanged();
            }
        }



        private void TitleOfNewNoteTextChanged(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            TitleOfNewNoteText = box.Text;
            SaveOrClose(TitleOfNewNoteText, ContentOfNewNoteText);
        }



        //--Content---
        private string _contentOfNewNoteText = "";

        public string ContentOfNewNoteText
        {
            get { return _contentOfNewNoteText; }
            set
            {
                _contentOfNewNoteText = value;
                OnPropertyChanged();
            }
        }

        private void ContentOfNewNoteGotFocus()
        {
            TitleOfNewNoteVisibility = NoteStyleOptionsVisibility = Visibility.Visible;

        }


        private void ContentOfNewNoteTextChanged(object sender, RoutedEventArgs e)
        {
            ContentOfNewNoteText = TextChangedFunction(ContentOfNewNote);
            SaveOrClose(TitleOfNewNoteText, ContentOfNewNoteText);

        }

        //---------Note Style Options-------------------------------------------------------------
        private Visibility _noteStyleOptionsVisibility = Visibility.Collapsed;
        public Visibility NoteStyleOptionsVisibility
        {
            get { return _noteStyleOptionsVisibility; }
            set
            {
                _noteStyleOptionsVisibility = value;
                OnPropertyChanged();
            }
        }

        //----Note Font Background

        private void FontBackgroundClick(object sender, RoutedEventArgs e)
        {
            UWPYourNoteLibrary.Util.NotesUtilities.FontBackgroundClick(ContentOfNewNote, null);
        }

        //----Note Font Increase
        private void FontIncreaseClick(object sender, RoutedEventArgs e)
        {
            UWPYourNoteLibrary.Util.NotesUtilities.FontIncreaseClick(ContentOfNewNote, null);
        }

        //----Note Font Decrease
        private void FontDecreaseClick(object sender, RoutedEventArgs e)
        {
            UWPYourNoteLibrary.Util.NotesUtilities.FontDecreaseClick(ContentOfNewNote, null);
        }


        //----Note Small Caps
        private void SmallCapsClick(object sender, RoutedEventArgs e)
        {
            UWPYourNoteLibrary.Util.NotesUtilities.SmallCapsClick(ContentOfNewNote, null);
        }


        //----Note All Caps
        private void AllCapsClick(object sender, RoutedEventArgs e)
        {
            UWPYourNoteLibrary.Util.NotesUtilities.AllCapsClick(ContentOfNewNote, null);
        }

        //----Note Strikethrough
        private void StrikethroughClick(object sender, RoutedEventArgs e)
        {
            UWPYourNoteLibrary.Util.NotesUtilities.StrikethroughClick(ContentOfNewNote, null);
        }
        //------------Creation/Close Button-------------------------

        private string _creationButtonContent = "Close";
        public string CreationButtonContent
        {
            get { return _creationButtonContent; }
            set
            {
                _creationButtonContent = value;
                OnPropertyChanged();
            }
        }


        private void CreationButtonClick()
        {
            if (CreationButtonContent == "Save")
            {


                if (String.IsNullOrEmpty(TitleOfNewNoteText))
                {
                    NoTitle();
                    return;

                }
                

                string creationDay = DateTime.Now.ToString("MMM/dd/yyyy hh:mm:ss.fff tt");
                _homePageViewModel = new HomePageViewModel();
                UWPYourNoteLibrary.Models.Note newNote = new UWPYourNoteLibrary.Models.Note(LoggedUser.userId, TitleOfNewNoteText, ContentOfNewNoteText, NoteEditOptions.ColorOptionsSelectedIndex, creationDay, creationDay);
                long noteId = _homePageViewModel.CreateNewNote(newNote);
                if (_notesDataItemSource == null)
                {
                    _notesDataItemSource = new ObservableCollection<UWPYourNoteLibrary.Models.Note>();

                }
                newNote.noteId = noteId;
                //  newNote.noteId = _homePageViewModel.GetNoteId(newNote.noteId, newNote.userId);
                _notesDataItemSource.Insert(0, newNote);
                NotesDataItemSource = _notesDataItemSource;
                ContentOfNewNoteText = "";
                TitleOfNewNoteText = "";
                ContentOfNewNote.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
            }



            TitleOfNewNoteVisibility = Visibility.Collapsed;
            NoteStyleOptionsVisibility = Visibility.Collapsed;


        }

        //----------------------------Note Grid View---------------------------------------------------

        private ObservableCollection<UWPYourNoteLibrary.Models.Note> _notesDataItemSource = null;
        public ObservableCollection<UWPYourNoteLibrary.Models.Note> NotesDataItemSource
        {
            get { return _notesDataItemSource; }
            set
            {
                _notesDataItemSource = value;
                OnPropertyChanged();
            }
        }

        public void NotesDataItemClick(object sender, ItemClickEventArgs e)
        {
            NoteDisplayPopUpOpened();
            selectedNoteFromDisplay = (UWPYourNoteLibrary.Models.Note)e.ClickedItem;
            NoteContentPopUp.DisplayContent(LoggedUser.userId, selectedNoteFromDisplay.noteId, selectedNoteFromDisplay.title, selectedNoteFromDisplay.content, selectedNoteFromDisplay.noteColor, selectedNoteFromDisplay.modifiedDay);


        }






        //----------------------------Note Display Popup---------------------------------------------------
        private void IsCloseAutoSave()
        {

        }

        private bool _noteDisplayPopUpIsLight = true;

        public bool NoteDisplayPopUpIsLight
        {
            get { return _noteDisplayPopUpIsLight; }
            set
            {
                _noteDisplayPopUpIsLight = value;
                OnPropertyChanged();
            }
        }


        private bool _noteDisplayPopUpIsOpen = false;
        public bool NoteDisplayPopUpIsOpen
        {
            get { return _noteDisplayPopUpIsOpen; }
            set
            {
                _noteDisplayPopUpIsOpen = value;
                //if (_noteDisplayPopUpIsOpen == false)
                //{
                //    IsCloseAutoSave();
                //}
                OnPropertyChanged();
            }
        }



        private double _noteContentPopUpHeight;
        public double NoteContentPopUpHeight
        {
            get { return _noteContentPopUpHeight; }
            set
            {
                _noteContentPopUpHeight = value;
                OnPropertyChanged();
            }
        }



        private double _noteContentPopUpWidth;
        public double NoteContentPopUpWidth
        {
            get { return _noteContentPopUpWidth; }
            set
            {
                _noteContentPopUpWidth = value;
                OnPropertyChanged();
            }
        }

        private void NoteDisplayPopUpLayoutUpdated(object sender, object e)
        {
            NoteContentPopUpHeight = Window.Current.Bounds.Height;
            NoteContentPopUpWidth = Window.Current.Bounds.Width/2;
            if (NoteContentPopUp.ActualWidth == 0 && NoteContentPopUp.ActualHeight == 0)
            {
                return;
            }

            var coordinates = NoteDisplayPopUp.TransformToVisual(Window.Current.Content).TransformPoint(new Windows.Foundation.Point(0, 0));

            double ActualHorizontalOffset = NoteDisplayPopUp.HorizontalOffset;
            double ActualVerticalOffset = NoteDisplayPopUp.VerticalOffset;

            double NewHorizontalOffset = ((Window.Current.Bounds.Width - NoteContentPopUp.ActualWidth) / 2) - coordinates.X;
            double NewVerticalOffset = ((Window.Current.Bounds.Height - NoteContentPopUp.ActualHeight) / 2) - coordinates.Y;

            if (ActualHorizontalOffset != NewHorizontalOffset || ActualVerticalOffset != NewVerticalOffset)
            {
                this.NoteDisplayPopUp.HorizontalOffset = NewHorizontalOffset;
                this.NoteDisplayPopUp.VerticalOffset = NewVerticalOffset;
            }
        }


        public void NoteDisplayPopUpOpened()
        {
            // PopOut.Stop();
             if (!NoteDisplayPopUpIsOpen)
            {
                NoteContentPopUpIsTapped = true;

                NoteDisplayPopUpIsOpen = true;
              //  PopIn.Begin();
            }
        }


        private void NoteDisplayPopUpClosed(object sender, object e)
        {
            //PopIn.Stop();
            NoteContentPopUp.ChangesOnClosing();

            NoteContentPopUp.UsersToShare = null;
            if (NoteContentPopUp._dispatcherTimer != null)
            {
                if (NoteContentPopUp.isDeleted)
                {
                    NoteContentPopUp.DispatcherTimerStop(NoteContentPopUp._dispatcherTimer);
                }
                else
                {
                    NoteContentPopUp.DispatcherTimer_Tick(sender, e);
                    NoteContentPopUp.DispatcherTimerStop(NoteContentPopUp._dispatcherTimer);

                }
            }
            if (NoteContentPopUp.isDeleted)
            {
                int i = NotesDataItemSource.IndexOf(selectedNoteFromDisplay);
                if (i != -1)
                    NotesDataItemSource.RemoveAt(i);
            }
            if (NoteContentPopUp.isDeleted == false && NoteContentPopUp.isModified)
            {

                int i = NotesDataItemSource.IndexOf(selectedNoteFromDisplay);
                Note note = NotesDataItemSource[i];
                NotesDataItemSource.RemoveAt(i);
                note.content = NoteContentPopUp.ContentOfNoteText;
                note.title = NoteContentPopUp.TitleOfNoteText;
                note.modifiedDay = NoteContentPopUp.currentDay;
                note.noteColor = NoteContentPopUp.GetNoteColor();


                NotesDataItemSource.Insert(0, note);
            }
            NoteDisplayPopUpIsOpen = false;

        }




        private bool _noteContentPopUpIsTapped = true;

        public bool NoteContentPopUpIsTapped
        {
            get { return _noteContentPopUpIsTapped; }
            set
            {
                _noteContentPopUpIsTapped = value;
                OnPropertyChanged();
            }
        }

        private void NoteContentPopUpTapped()
        {
            if (NoteContentPopUpIsTapped)
            {
                NoteContentPopUpIsTapped = false;
                NoteContentPopUp.EditModeEnabled();
            }
                
        }



        private void NoteEditOptions_EditOptions(string btnName)
        {

            switch (btnName)
            {
                case "FontBackground": FontBackgroundClick(null, null); break;
                case "FontIncrease": FontIncreaseClick(null, null); break;
                case "FontDecrease": FontDecreaseClick(null, null); break;
                case "SmallCaps": SmallCapsClick(null, null); break;
                case "AllCaps": AllCapsClick(null, null); break;
                case "Strikethrough": StrikethroughClick(null, null); break;
                default: return;
            }

        }


    }
}
