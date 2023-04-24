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
using Windows.Media.Protection;
using Windows.System;
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
using static System.Net.Mime.MediaTypeNames;
using YourNoteUWP.ViewModels.Contract;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using static UWPYourNoteLibrary.Util.NotesUtilities;
using UWPYourNoteLibrary.Data.Handler.Contract;
using UWPYourNoteLibrary.Data.Handler;
using Windows.Media.Playback;
using UWPYourNote.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPYourNote.View.usercontrol
{
    public sealed partial class NoteContent : UserControl, INoteContentView, INotifyPropertyChanged
    {
        

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ToEnableEditMode()
        {
            _dispatcherTimer = null;
            NoteMenuOptions.UsersToShare = null;
           

        }

        private void TakeNoteColor(long color)
        {
            NoteContentBackground = NotesUtilities.GetSolidColorBrush(color);
            NoteMenuOptions.NoteColorForeground = NotesUtilities.GetSolidColorBrush(color);
            NoteMenuOptions.ColorOptionsSelectedIndex = (int)color;
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




        ////---------------------------Note Background--------------------------------------------------

        private void NoteColorChosenChanged()
        {
            noteContentVM.noteContentView = this;
            _noteColorChosen = NoteMenuOptions.ColorOptionsSelectedIndex;
            string modifiedDay = DateTime.Now.ToString("MMM/dd/yyyy hh:mm:ss.fff tt");
            noteContentVM.UpdateNoteColor(DisplayNote.noteId, _noteColorChosen, modifiedDay);
        }

        private SolidColorBrush _noteContentBackground;
        public SolidColorBrush NoteContentBackground
        {
            get { return _noteContentBackground; }
            set
            {
                _noteContentBackground = value;

                OnPropertyChanged();
                NoteColorChosenChanged();

            }
        }

        ////----------------------------Note Close Button ---------------------------------------------------


        private System.Delegate _delPageMethod;
        public Delegate CallingPageMethod
        {
            set { _delPageMethod = value; }
        }


        public void NoteCloseButtonClick(object sender, RoutedEventArgs e)
        {
            _delPageMethod.DynamicInvoke(null, null);

        }

        ////--------------------------- NoteMenuOptions  ------------------------------------------


        private SolidColorBrush _noteMenuOptionsContainerBackground;

        public SolidColorBrush NoteMenuOptionsContainerBackground
        {
            get { return NoteMenuOptionsContainerBackground; }
            set { NoteMenuOptionsContainerBackground = value; }
        }

        ////----------------------------Note Share Button ---------------------------------------------------

        private ObservableCollection<UWPYourNoteLibrary.Models.User> _usersToShare;

        public ObservableCollection<UWPYourNoteLibrary.Models.User> UsersToShare
        {
            get { return _usersToShare; }
            set
            {
                _usersToShare = value;
                OnPropertyChanged();
            }
        }

        public void NoteShareButtonClick(object sender, RoutedEventArgs e)
        {
            noteContentVM = NoteContentVM.Singleton;
            noteContentVM.noteContentView = this;
            noteContentVM.IsOwner(DisplayNote.userId, DisplayNote.noteId);

        }

        ////----------------------------Note Delete Button ---------------------------------------------------
        public void NoteDeleteButtonClick(object sender, RoutedEventArgs e)
        {

            noteContentVM.noteContentView = this;
            noteContentVM.DeleteNote(DisplayNote.noteId);
            _delPageMethod?.DynamicInvoke(null, null);
        }
        ////----------------------------Content Text Box ---------------------------------------------------



        public void CanShareNote(bool result)
        {
            noteContentVM.GetUsersToShare(DisplayNote.userId, DisplayNote.noteId);
        }

        public  void ValidUsersLists(ObservableCollection<UWPYourNoteLibrary.Models.User> listOfUsers)
        {
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
             {
                 if (listOfUsers == null)
                 {
                     NoValidUsers();
                     return;
                 }
                 NoteMenuOptions.UsersToShare = listOfUsers;
                 //foreach (UWPYourNoteLibrary.Models.User user in listOfUsers)
                 //{
                 //    NoteMenuOptions.UsersToShare.Add(user);
                 //}
             });
        }







        private void UsersToShareViewItemClick(object sender, ItemClickEventArgs e)
        {
            noteContentVM.noteContentView = this;
            UWPYourNoteLibrary.Models.User selectedUser = (UWPYourNoteLibrary.Models.User)e.ClickedItem;
            noteContentVM.ShareNote(selectedUser.userId, DisplayNote.noteId);

        }



        ////----------------------------------------Auto Save----------------------------------------




        private void NoteBackgroundColor()
        {
            NoteContentBackground = NoteMenuOptions.NoteColorForeground;

        }




        private void PopOutButton()
        {
            DisplayNote.title = TitleOfNoteText;
            DisplayNote.content = ContentOfNoteText;
            DisplayNote.modifiedDay = DateTime.Now.ToString("MMM/dd/yyyy hh:mm:ss.fff tt");
            DisplayNote.noteColor = NoteMenuOptions.ColorOptionsSelectedIndex;
            PopOutWindow?.Invoke(DisplayNote);
            NoteCloseButtonClick(null, null);
        }

        NoteContentVM noteContentVM = null;
        
        public DispatcherTimer _dispatcherTimer;
        private string _oldTitle ;
        private string _oldContent ;
        public long _noteColorChosen;

        public event Action<Note> PopOutWindow;
        private delegate void NoteContentUserControl(object sender, RoutedEventArgs e);
        private delegate void ToShareView(object sender, ItemClickEventArgs e);
        public NoteContent()
        {
            this.InitializeComponent();
            noteContentVM = NoteContentVM.Singleton;

            NoteContentUserControl delUserControlMethod = new NoteContentUserControl(NoteShareButtonClick);
            NoteMenuOptions.CallingPageMethod = delUserControlMethod;

            ToShareView itemClick = new ToShareView(UsersToShareViewItemClick);
            NoteMenuOptions.ToShare = itemClick;

        }

        private Note _displayNote = null;
        public Note DisplayNote
        {
            get { return _displayNote; }
            set
            {
                _displayNote = value;
                OnPropertyChanged();
            }
        }

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
        private void UpdateCount(long searchCount)
        {
            noteContentVM.noteContentView = this;
            noteContentVM.UpdateCount(searchCount, DisplayNote.noteId);
        }
        public void EditMode()
        {
            _dispatcherTimer = null;
            _oldContent = _oldTitle  = null;
            _noteColorChosen = DisplayNote.noteColor;
            NoteMenuOptions.MinimizeVisibility = NoteMenuOptions.PopOutVisibility = Visibility.Visible;
            TitleOfNoteText = _oldTitle = DisplayNote.title;
            ContentOfNoteText = _oldContent = DisplayNote.content;
            ContentOfNote.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, ContentOfNoteText);
            TakeNoteColor(DisplayNote.noteColor);
            DispatcherTimerStart();

        }

        public void DisplayContent(Note displayNote, long searchCount = -1)
        {
            try
            {
                DisplayNote = displayNote;
                if (searchCount != -1)
                {
                    DisplayNote.searchCount = searchCount;
                    UpdateCount(searchCount);
                }
                EditMode();
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message);
            }
        }


        private void DispatcherTimerStart()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = new TimeSpan(0, 0, (int)AutoSaveTimer.TimeFrequency);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            _dispatcherTimer.Start();
        }

        private bool IsChanged(string oContext, string nContext)
        {
            bool isChange = false;
            if (oContext != nContext)
                isChange = true;
            return isChange;
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
                noteContentVM.noteContentView = this;
                noteContentVM.UpdateNote(DisplayNote, titleChange, contentChange);

            }
        }

        public void DispatcherTimerStop()
        {
            _dispatcherTimer.Tick -= DispatcherTimer_Tick;
            _dispatcherTimer.Stop();
            _dispatcherTimer = null;
        }



        private void ContentOfNoteTextChanged(object sender, RoutedEventArgs e)
        {
            RichEditBox Context = (RichEditBox)sender;
            string text = null;
            Context.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out text);
            ContentOfNoteText = text;

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
                case "PopOutButton": PopOutButton(); break;

                case "True":
                case "False":
                    {
                        if ("True" == name)
                            noteContentVM.IsNoteShared(true);
                        else
                            noteContentVM.IsNoteShared(false);
                        break;

                    }
                default: return;
            }
        }



    }





}

