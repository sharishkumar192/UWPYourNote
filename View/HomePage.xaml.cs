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
using UWPYourNote.ViewModels.Contract;
using UWPYourNote.ViewModels.usercontrol;
using static UWPYourNoteLibrary.Util.NotesUtilities;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using UWPYourNote.View.usercontrol;
using YourNoteUWP.View;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Hosting;
using System.Reflection;
using YourNoteUWP.ViewModels.Contract;
using UWPYourNoteLibrary.Notification;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPYourNote.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page, IHomePageView, INotifyPropertyChanged
    {
        private Frame _frame;
        private UWPYourNoteLibrary.Models.Note _selectedNote = null;
        static UWPYourNoteLibrary.Models.Note selectedNoteFromDisplay = null;
        private HomePageVM homePageVM;
        private NoteTitleSearchVM noteTitleSearchVM;
        bool themeCheck;
        bool accentCheck;
        bool isTitleGotFocus = false;
        Dictionary<long, int> NoteApplicationView = new Dictionary<long, int>();


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public HomePage()
        {

            themeCheck = accentCheck = false;
            this.InitializeComponent();
            TitleSearchBox.SearchTextBoxText = string.Empty;
        }


        private delegate void DelUserControlMethod(object sender, object e);
        private delegate void AccentChange(object sender, object e);
        private void DelegateIntialize()
        {
            DelUserControlMethod delUserControlMethod = new DelUserControlMethod(NoteDisplayPopUpClosed);
            NoteContentPopUp.CallingPageMethod = delUserControlMethod;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DelegateIntialize();
            homePageVM = HomePageVM.Singleton;
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
                //    TitleSearchBox.LoggedUser = value;
                OnPropertyChanged();
            }

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


        public void GetNotes(TypeOfNote type)
        {
            homePageVM.homePageView = this;
            homePageVM.GetNotes(LoggedUser.userId, true, type);

        }







        //----------------------------Main Menu List Box---------------------------------------------------

        private string _welcomeText = "Welcome, ";

        public string WelcomeText
        {
            get { return _welcomeText + LoggedUser.name; }
            set
            {
                _welcomeText = value;

                OnPropertyChanged();
            }
        }


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
            ListBoxItem item = (ListBoxItem)box.SelectedItem;
            MainMenuOptionsSelectedIndex = box.SelectedIndex;
            TypeOfNote typeOfNote;
            TitleSearchBox.SearchTextBoxText = "";
            // TitleSearchBox.SearchPopupIsOpen = false;

            if (PersonalNotesIsSelected == true)
            {


                SharedNotesIsSelected = AllNotesIsSelected = false;
                typeOfNote = TypeOfNote.PersonalNotes;
                TitleText = typeOfNote.ToString();
                GetNotes(typeOfNote);


                TitleSearchBox._selectedNote = new Note("", "", "", 0);

            }
            else if (SharedNotesIsSelected == true)
            {

                PersonalNotesIsSelected = AllNotesIsSelected = false;
                typeOfNote = TypeOfNote.SharedNotes;
                TitleText = typeOfNote.ToString();
                GetNotes(typeOfNote);
                TitleSearchBox._selectedNote = new Note("", "", "", 0);

            }
            else if (AllNotesIsSelected == true)
            {
                PersonalNotesIsSelected = SharedNotesIsSelected = false;
                typeOfNote = TypeOfNote.AllNotes;
                TitleText = typeOfNote.ToString();
                GetNotes(typeOfNote);
            }

        }

        //----------------------------Search Text Box---------------------------------------------------
        public void SearchBoxContainerLostFocus()
        {
            //  TitleSearchBox.SearchPopupIsOpen = false;
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










        //----------------------------Search -> Suggestion List homePageView ---------------------------------------------------






        //----------------------------Sign Out Button---------------------------------------------------
        public void LogoutContentTapped()
        {
            OnPageExited();
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
        private SolidColorBrush _newNoteBackground = NotesUtilities.GetSolidColorBrush(0);

        public SolidColorBrush NewNoteBackground
        {
            get { return _newNoteBackground; }
            set
            {
                _newNoteBackground = value;
                OnPropertyChanged();
            }
        }


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
            if (!isTitleGotFocus)
            {
                TitleOfNewNote.Focus(FocusState.Programmatic);
                isTitleGotFocus = true;
            }
            // TitleOfNewNote.Select(0, 0);

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
            NotesUtilities.FontBackgroundClick(ContentOfNewNote, null);
        }

        //----Note Font Increase
        private void FontIncreaseClick(object sender, RoutedEventArgs e)
        {
            NotesUtilities.FontIncreaseClick(ContentOfNewNote, null);
        }

        //----Note Font Decrease
        private void FontDecreaseClick(object sender, RoutedEventArgs e)
        {
            NotesUtilities.FontDecreaseClick(ContentOfNewNote, null);
        }


        //----Note Small Caps
        private void SmallCapsClick(object sender, RoutedEventArgs e)
        {
            NotesUtilities.SmallCapsClick(ContentOfNewNote, null);
        }


        //----Note All Caps
        private void AllCapsClick(object sender, RoutedEventArgs e)
        {
            NotesUtilities.AllCapsClick(ContentOfNewNote, null);
        }

        //----Note Strikethrough
        private void StrikethroughClick(object sender, RoutedEventArgs e)
        {
            NotesUtilities.StrikethroughClick(ContentOfNewNote, null);
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

        private void CreateNewNote()
        {
            string creationDay = DateTime.Now.ToString("MMM/dd/yyyy hh:mm:ss.fff tt");
            UWPYourNoteLibrary.Models.Note newNote = new UWPYourNoteLibrary.Models.Note(LoggedUser.userId, TitleOfNewNoteText, ContentOfNewNoteText, NoteEditOptions.ColorOptionsSelectedIndex, creationDay, creationDay);
            homePageVM.homePageView = this;
            homePageVM.CreateNewNote(newNote);
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

                CreateNewNote();
                ContentOfNewNoteText = "";
                TitleOfNewNoteText = "";
                ContentOfNewNote.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
            }

            TitleOfNewNoteVisibility = Visibility.Collapsed;
            NoteStyleOptionsVisibility = Visibility.Collapsed;
            isTitleGotFocus = false;
        }

        //----------------------------Note Grid homePageView---------------------------------------------------

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
            //   ColumnSplitter.Visibility = DetailsViewVisibility = Visibility.Visible;
            //   Tuple<Note, Frame> tp = new Tuple<Note, Frame>(selectedNoteFromDisplay, DetailsView);
            //  DetailsView.Navigate(typeof(NoteDisplayApplicationView), tp);
            double width = this.ActualWidth;
            if (width < 600)
            {
                NotesInPopOutWindow(selectedNoteFromDisplay);
                return;
            }
            selectedNoteFromDisplay.userId = LoggedUser.userId;
            NoteContentPopUp.DisplayContent(selectedNoteFromDisplay);


        }

        private Visibility _detailsViewVisibility = Visibility.Collapsed;

        public Visibility DetailsViewVisibility
        {
            get { return _detailsViewVisibility; }
            set
            {
                _detailsViewVisibility = value;
                OnPropertyChanged();
            }
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
            NoteContentPopUpHeight = Window.Current.Bounds.Height * 1.5 / 2;
            NoteContentPopUpWidth = Window.Current.Bounds.Width / 2;
            //   NoteContentPopUpHeight = Window.Current.Bounds.Height;
            //   NoteContentPopUpWidth = Window.Current.Bounds.Width;
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
            NoteContentPopUp.UsersToShare = null;

            if (NoteContentPopUp._dispatcherTimer != null)
            {
                NoteContentPopUp.DispatcherTimer_Tick(sender, e);
                NoteContentPopUp.DispatcherTimerStop();
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




        private void NoteBackgroundColor()
        {
            NewNoteBackground = NoteEditOptions.NoteColorForeground;

        }


        private void NoteEditOptionsEditOptions(string btnName)
        {

            switch (btnName)
            {
                case "FontBackground": FontBackgroundClick(null, null); break;
                case "FontIncrease": FontIncreaseClick(null, null); break;
                case "FontDecrease": FontDecreaseClick(null, null); break;
                case "SmallCaps": SmallCapsClick(null, null); break;
                case "AllCaps": AllCapsClick(null, null); break;
                case "Strikethrough": StrikethroughClick(null, null); break;
                case "ColorOptions": NoteBackgroundColor(); break;
                default: return;
            }

        }


        private async void AppearenceClick(object sender, RoutedEventArgs e)
        {
            AppWindow appWindow = await AppWindow.TryCreateAsync();
            Frame appWindowContentFrame = new Frame();
            appWindowContentFrame.Navigate(typeof(Appearence));
            ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);
            await appWindow.TryShowAsync();
            var id = ApplicationView.GetForCurrentView().Id;



        }

        private void ThemeToggleButtonClick()
        {
            SettingsFlyout.Hide();
        }



        private void TitleSearchBoxDisplaySelectedNote(Note selectedNote)
        {
            selectedNote.userId = LoggedUser.userId;
            double width = this.ActualWidth;
            if (width < 600)
            {
                NotesInPopOutWindow(selectedNote);
                return;
            }
            NoteContentPopUp.DisplayContent(selectedNote, selectedNote.searchCount);
            NoteDisplayPopUpOpened();
        }

        private void NoteSearchButton_Click(object sender, RoutedEventArgs e)
        {
            double width = this.ActualWidth;
            double height = this.ActualHeight;
            if (width > 600 && height > 0)
            {
                AppName.Visibility = AppLogo.Visibility = Visibility.Visible;
            }
            TitleSearchBoxContainer.Visibility = Visibility.Visible;
            NoteSearchButton.Visibility = MainMenuOptions.Visibility = Visibility.Collapsed;
            TitleSearchBox.Focus(FocusState.Programmatic);
        }

        private void TitleSearchBox_SearchTextBoxLostFocus()
        {
            double width = this.ActualWidth;
            double height = this.ActualHeight;
            if (width > 1100 && height > 0)
            {
                AppName.Visibility = AppLogo.Visibility = TitleSearchBoxContainer.Visibility = MainMenuOptions.Visibility = Visibility.Visible;
                NoteSearchButton.Visibility = Visibility.Collapsed;
                return;
            }
            AppName.Visibility = AppLogo.Visibility = TitleSearchBoxContainer.Visibility = Visibility.Collapsed;
            NoteSearchButton.Visibility = MainMenuOptions.Visibility = Visibility.Visible;
        }






        private void TitleOfNewNote_LosingFocus(UIElement sender, LosingFocusEventArgs args)
        {

        }

        private void TitleOfNewNote_LostFocus(object sender, RoutedEventArgs e)
        {
            //  ContentOfNewNote.Focus(FocusState.Programmatic);
        }



        public async void NotesInPopOutWindow(Note selectedNote)
        {
            selectedNoteFromDisplay = null;
            selectedNoteFromDisplay = selectedNote;
            if (NoteApplicationView.ContainsKey(selectedNote.noteId))
            {
                await ApplicationViewSwitcher.SwitchAsync(NoteApplicationView[selectedNote.noteId]);
                return;
            }
            int newViewId = 0;
            CoreApplicationView newView = CoreApplication.CreateNewView();
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(NoteDisplayApplicationView), selectedNote);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
                newView.CoreWindow.Closed += CoreWindow_Closed;

            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId, ViewSizePreference.UseLess);
            NoteApplicationView[selectedNote.noteId] = newViewId;


        }

        private void CoreWindow_Closed(CoreWindow sender, CoreWindowEventArgs args)
        {
            CoreWindow coreWindow = sender as CoreWindow;
            // coreWindow.UsersToShare = null;

            if (NoteContentPopUp._dispatcherTimer != null)
            {
                NoteContentPopUp.DispatcherTimer_Tick(sender, null);
                NoteContentPopUp.DispatcherTimerStop();
            }
            NoteDisplayPopUpIsOpen = false;
        }

        private void NewView_HostedViewClosing(CoreApplicationView sender, HostedViewClosingEventArgs args)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NotificationManager.UpdateNoteSucceeded += NotificationManager_UpdateNoteSucceeded;
            NotificationManager.DeleteNoteSucceeded += NotificationManager_DeleteNoteSucceeded;
        }



        private void OnPageExited()
        {
            NotificationManager.UpdateNoteSucceeded -= NotificationManager_UpdateNoteSucceeded;
            NotificationManager.DeleteNoteSucceeded -= NotificationManager_DeleteNoteSucceeded;
        }

        private void NotificationManager_UpdateNoteSucceeded(Note updateNote)
        {
            homePageVM.homePageView = this;
            homePageVM.NoteUpdation(selectedNoteFromDisplay, updateNote);
        }

        private void NotificationManager_DeleteNoteSucceeded()
        {
            homePageVM.homePageView = this;
            homePageVM.NoteDeletion(selectedNoteFromDisplay);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
