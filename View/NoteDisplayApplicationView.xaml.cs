using System;
using System.Collections.Generic;
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
using UWPYourNoteLibrary.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UWPYourNote.View.usercontrol;
using UWPYourNoteLibrary.Util;
using Windows.UI.Popups;
using UWPYourNote.ViewModels;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using static UWPYourNoteLibrary.Util.NotesUtilities;
using System.Collections.ObjectModel;
using UWPYourNote.ViewModels.Contract;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPYourNote.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoteDisplayApplicationView : Page, INotifyPropertyChanged, INoteApplicationView
    {
        public DispatcherTimer _dispatcherTimer = null;
        private string _oldContent = "";
        private string _oldTitle = "";
        public NoteDisplayApplicationViewVM noteDisplayApplicationVM;
        private delegate ObservableCollection<UWPYourNoteLibrary.Models.User> NoteContentUserControl(object sender, RoutedEventArgs e);
        private delegate void ToShareView(object sender, ItemClickEventArgs e);


       
        public NoteDisplayApplicationView()
        {
            this.InitializeComponent();
            noteDisplayApplicationVM = new NoteDisplayApplicationViewVM();
           
          //  Windows.UI.Core.CoreApplication.Current.Suspending += Current_Suspending;
        }
    

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private void UsersToShareViewItemClick(object sender, ItemClickEventArgs e)
        {
            UWPYourNoteLibrary.Models.User selectedUser = (UWPYourNoteLibrary.Models.User)e.ClickedItem;
            noteDisplayApplicationVM.noteApplicationView = this;
            noteDisplayApplicationVM.ShareNote(selectedUser.userId, DisplayNote.noteId);

        }
     

        public void NoteShareButtonClick(object sender, RoutedEventArgs e)
        {

            noteDisplayApplicationVM = NoteDisplayApplicationViewVM.Singleton;
            noteDisplayApplicationVM.noteApplicationView = this;
            noteDisplayApplicationVM.IsOwner(DisplayNote.userId, DisplayNote.noteId);

        }
     
        public void CanShareNote(bool result)
        {
            noteDisplayApplicationVM.GetUsersToShare(DisplayNote.userId, DisplayNote.noteId);
        }

        public void ValidUsersLists(ObservableCollection<UWPYourNoteLibrary.Models.User> listOfUsers)
        {
            if (listOfUsers == null)
            {
                NoValidUsers();
                return;
            }
            NoteMenuOptions.UsersToShare = listOfUsers;
        }

        private Note _displayNote = null ;

        public Note DisplayNote
        {
            get { return _displayNote; }
            set { _displayNote = value;
                OnPropertyChanged();
            }
        }

        private void TakeNoteColor(long color)
        {
            NoteContentBackground = NotesUtilities.GetSolidColorBrush(color);
            NoteMenuOptions.NoteColorForeground = NotesUtilities.GetSolidColorBrush(color);
            NoteMenuOptions.ColorOptionsSelectedIndex = (int)color;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            DisplayNote = (Note)e.Parameter;
            NoteContentBackground = NotesUtilities.GetSolidColorBrush(DisplayNote.noteColor);
            TitleOfNoteText  = _oldTitle = DisplayNote.title;
            
            ContentOfNoteText  = _oldContent  = DisplayNote.content;
            ContentOfNote.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, ContentOfNoteText);
            TakeNoteColor(DisplayNote.noteColor);
            DispatcherTimerStart();
        }

        //------------------------------------------------------------------



        private SolidColorBrush _noteContentBackground = NotesUtilities.GetSolidColorBrush(0);
        public SolidColorBrush NoteContentBackground
        {
            get { return _noteContentBackground; }
            set
            {
                _noteContentBackground = value;

                OnPropertyChanged();

            }
        }


        //----------------------------Title Text Box---------------------------------------------------
       


        private string _titleOfNoteText;
        public string TitleOfNoteText
        {
            get { return _titleOfNoteText; }
            set
            {
                _titleOfNoteText = value;
                OnPropertyChanged();
            }
        }
    
        //----------------------------Content Text Box ---------------------------------------------------

        private string _contentOfNoteText;

        public string ContentOfNoteText
        {
            get { return _contentOfNoteText; }
            set
            {
                _contentOfNoteText = value;
                OnPropertyChanged();
            }
        }



        //----------------------------------------Auto Save----------------------------------------


      
        public void DispatcherTimerStart()
        {

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, (int)AutoSaveTimer.TimeFrequency);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();

        }
        public void DispatcherTimerStop(DispatcherTimer dispatcherTimer)
        {
            dispatcherTimer.Tick -= DispatcherTimer_Tick;
            dispatcherTimer.Stop();
            dispatcherTimer = null;
        }


        public void DispatcherTimer_Tick(object sender, object e)
        {
            bool contentChange = IsChanged(_oldContent, ContentOfNoteText);
            bool titleChange = IsChanged(_oldTitle, TitleOfNoteText);
            if (titleChange)
            {
                DisplayNote.title = TitleOfNoteText;

            }
            if (contentChange)
            {
                DisplayNote.content = ContentOfNoteText;

            }
            if (titleChange || contentChange)
            {
                DisplayNote.noteColor = NoteMenuOptions.ColorOptionsSelectedIndex;
                DisplayNote.modifiedDay = DateTime.Now.ToString("MMM/dd/yyyy hh:mm:ss.fff tt");
                noteDisplayApplicationVM.UpdateNote(DisplayNote, titleChange, contentChange);

            }
        }

        private bool IsChanged(string oContext, string nContext)
        {
            bool isChange = false;
            if (oContext != nContext)
                isChange = true;
            return isChange;
        }

        private void TitleOfNoteTextChanged(object sender, RoutedEventArgs e)
        {

            TextBox box = (TextBox)sender;
            TitleOfNoteText = box.Text;


        }

        private void NoteMenuOptionsEditOptions(string name)
        {
            switch (name)
            {
                case "FontBackground": NotesUtilities.FontBackgroundClick(ContentOfNote, null); break;
                case "FontIncrease": NotesUtilities.FontIncreaseClick(ContentOfNote, null); break;
                case "FontDecrease": NotesUtilities.FontDecreaseClick(ContentOfNote, null); break;
                case "SmallCaps": NotesUtilities.SmallCapsClick(ContentOfNote, null); break;
                case "AllCaps": NotesUtilities.AllCapsClick(ContentOfNote, null); break;
                case "Strikethrough": NotesUtilities.StrikethroughClick(ContentOfNote, null); break;
                case "ColorOptions": NoteBackgroundColor(); break;
                case "NoteDeleteButton": NoteDeleteButtonClick(null, null); break;
                case "NoValidUsers": NoValidUsers(); break;
                case "True":
                case "False":
                    {
                        //if ("True" == name)
                        //    noteContentVM.IsNoteShared(true);
                        //else
                        //    noteContentVM.IsNoteShared(false);
                        break;

                    }
                default: return;
            }
        }

  

        private void ContentOfNoteTextChanged(object sender, RoutedEventArgs e)
        {
            RichEditBox Context = (RichEditBox)sender;
            string text;

            Context.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out text);

            ContentOfNoteText = text;

        }


        private void NoteBackgroundColor()
        {
            NoteContentBackground = NoteMenuOptions.NoteColorForeground;

        }

        private System.Delegate _delPageMethod;
        public Delegate CallingPageMethod
        {
            set { _delPageMethod = value; }
        }


        //----------------------------Note Delete Button ---------------------------------------------------
        public void NoteDeleteButtonClick(object sender, RoutedEventArgs e)
        {

            noteDisplayApplicationVM.DeleteNote(DisplayNote.noteId);
            _delPageMethod.DynamicInvoke(null, null);
           
        }


        private async void NoValidUsers()
        {
            MessageDialog showDialog;
            showDialog = new MessageDialog("No users available to share the note");
            showDialog.Commands.Add(new UICommand("Ok")
            {
                Id = 0
            });
            showDialog.DefaultCommandIndex = 0;
            var result = await showDialog.ShowAsync();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }



}
