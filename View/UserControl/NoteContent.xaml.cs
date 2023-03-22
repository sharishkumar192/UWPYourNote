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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPYourNote.View.usercontrol
{
    public sealed partial class NoteContent : UserControl,INoteContentView, INotifyPropertyChanged
    {
        //  private Type _parentPage;
        public event PropertyChangedEventHandler PropertyChanged;
        public DispatcherTimer _dispatcherTimer = null;
        private NoteContentVM noteContentVM;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


         private long _noteId;
        private long _searchCount;
    public string currentDay = "";
        private string _userId = "";
        public bool isModified = false;
        public bool isDeleted = false;
        private NoteContentVM _noteContentViewModel;

        private bool _gotCount = false;

        public long _noteColorChosen = 0;

        public bool  GotCount
        {
            get { return _gotCount; }
            set { _gotCount = value;
                UpdateCount();
            }
        }

        private void UpdateCount()
        {
                _noteContentViewModel = NoteContentVM.Singleton;
                _noteContentViewModel.UpdateCount(_searchCount, _noteId);
        }

        private delegate ObservableCollection<UWPYourNoteLibrary.Models.User> NoteContentUserControl(object sender, RoutedEventArgs e);


        public NoteContent()
        {
            this.InitializeComponent();

            noteContentVM = NoteContentVM.Singleton;
            noteContentVM.noteContentView = this;
            TitleOfNote.AddHandler(TappedEvent, new TappedEventHandler(TitleOfNoteTapped), TitleOfNoteIsTapped);
              ContentOfNote.AddHandler(TappedEvent, new TappedEventHandler(ContentOfNoteTapped), ContentOfNoteIsTapped);


            NoteContentUserControl delUserControlMethod = new NoteContentUserControl(NoteShareButtonClick);
           NoteMenuOptions.CallingPageMethod = delUserControlMethod;

            ToShareView itemClick = new ToShareView(UsersToShareView_ItemClick);
            NoteMenuOptions.ToShare = itemClick;

        }
        public void ChangesOnClosing()
        {
            ContentOfNoteIsReadOnly = false;
        }
        private void ToEnableEditMode()
        {
            NoteMenuOptionsVisibility = Visibility.Collapsed;
            TitleOfNoteIsReadOnly = true;
            ContentOfNoteIsReadOnly = true;
            TitleOfNoteIsTapped = true;
            ContentOfNoteIsTapped = true;
            _dispatcherTimer = null;
            isModified = false;
            NoteMenuOptions.UsersToShare = null;
            isDeleted = false;
     

        }

        private void TakeNoteColor(long color)
        {
            NoteContentBackground = NotesUtilities.GetSolidColorBrush(color);
            NoteMenuOptions.NoteColorForeground = NotesUtilities.GetSolidColorBrush(color);
            NoteMenuOptions.ColorOptionsSelectedIndex = (int)color;
        }
        public void DisplayContent(string userId, long noteId, string title, string content, long noteColor, string modifiedDay)
        {
            try { 
            _noteId = noteId;
            _userId = userId;
            TitleOfNoteText = title;
            ContentOfNoteText = content;
            currentDay = modifiedDay;
                _noteColorChosen = noteColor;

             
             ContentOfNote.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, ContentOfNoteText);
                TakeNoteColor(noteColor);
                ToEnableEditMode();
            }
            catch(Exception ex)
            {
                Logger.WriteLog(ex.Message);
            }   

        }

        public void DisplayContent(string userId, long noteId, string title, string content, long searchCount, long noteColor, string modifiedDay)
        {
            _noteId = noteId;
            _userId = userId;
            TitleOfNoteText = title;
            ContentOfNoteText = content;
            _searchCount = searchCount;
            GotCount = true;
              currentDay = modifiedDay;
            TakeNoteColor(noteColor);
            ContentOfNote.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, ContentOfNoteText);
            ToEnableEditMode();
        }






        private async void NoteShared(bool value)
        {
            MessageDialog showDialog;
            if(value == true)
            showDialog = new MessageDialog("Note has been shared!");
            else
                showDialog = new MessageDialog("You cant share this note, as your not the owner!");
            showDialog.Commands.Add(new UICommand("Ok")
            {
                Id = 0
            });
            showDialog.DefaultCommandIndex = 0;
            var result = await showDialog.ShowAsync();
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




        //---------------------------Note Background--------------------------------------------------

        private void NoteColorChosenChanged()
        {
            _noteContentViewModel = NoteContentVM.Singleton;
              _noteColorChosen = NoteMenuOptions.ColorOptionsSelectedIndex;
        isModified = true;
            currentDay = DateTime.Now.ToString("MMM/dd/yyyy hh:mm:ss.fff tt");
            _noteContentViewModel.ChangeNoteColor(_noteId, _noteColorChosen, currentDay);
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


        private bool _noteContentIsTapped = true;

        public bool NoteContentIsTapped
        {
            get { return _noteContentIsTapped; }
            set { _noteContentIsTapped = value;
                OnPropertyChanged();
            }
        }

        private void NoteContentTapped()
        {
            if (NoteContentIsTapped)
                EditModeEnabled();
        }




        //----------------------------Title Text Box---------------------------------------------------
        private bool _titleOfNoteIsReadOnly = true;
        public bool TitleOfNoteIsReadOnly
        {
            get { return _titleOfNoteIsReadOnly; }
            set
            {
                _titleOfNoteIsReadOnly = value;
                OnPropertyChanged();
            }
        }
        private bool _titleOfNoteIsTapped = true;
        public bool TitleOfNoteIsTapped
        {
            get { return _titleOfNoteIsTapped; }
            set
            {
                _titleOfNoteIsTapped = value;
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

        private string _oldTitle = "";




        //----------------------------Note Close Button ---------------------------------------------------
        //public event EventHandler UserControlButtonClicked;

        //private void OnNoteDisplayPopUpClosed(bool value)
        //{
        //    if (UserControlButtonClicked != null)
        //    {
        //        UserControlButtonClicked(this, value);
        //    }
        //}

        private System.Delegate _delPageMethod;
        public Delegate CallingPageMethod
        {
            set { _delPageMethod = value;  }
        }


        public void NoteCloseButtonClick(object sender, RoutedEventArgs e)
        {
            _delPageMethod.DynamicInvoke(null, null);
            
         }

        //--------------------------- NoteMenuOptions  ------------------------------------------
        private Visibility _NoteMenuOptionsVisibility = Visibility.Collapsed;

        public Visibility NoteMenuOptionsVisibility
        {
            get { return _NoteMenuOptionsVisibility; }
            set
            {
                _NoteMenuOptionsVisibility = value;
                OnPropertyChanged();
            }
        }

        private SolidColorBrush _noteMenuOptionsContainerBackground;

        public SolidColorBrush NoteMenuOptionsContainerBackground
        {
            get { return NoteMenuOptionsContainerBackground; }
            set { NoteMenuOptionsContainerBackground = value; }
        }

        //----------------------------Note Share Button ---------------------------------------------------

        private ObservableCollection<UWPYourNoteLibrary.Models.User> _usersToShare;

        public ObservableCollection<UWPYourNoteLibrary.Models.User> UsersToShare
        {
            get { return _usersToShare; }
            set { _usersToShare = value;
                OnPropertyChanged();
            }
        }
     
        public ObservableCollection<UWPYourNoteLibrary.Models.User> NoteShareButtonClick(object sender, RoutedEventArgs e)
        {
            ObservableCollection<UWPYourNoteLibrary.Models.User> notes = null;
            _noteContentViewModel = NoteContentVM.Singleton;
            if (_noteContentViewModel.IsOwner(_userId, _noteId) == true)
            {
          notes = _noteContentViewModel.GetUsersToShare(_userId, _noteId);

            }
            else
            {
                NoteShared(false);
            }

            return notes;
        }

        //----------------------------Note Delete Button ---------------------------------------------------
        public void NoteDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            
            
            _noteContentViewModel = NoteContentVM.Singleton;
            _noteContentViewModel.DeleteNote(_noteId);
            isDeleted = true;
            _delPageMethod.DynamicInvoke(null, null);
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

        private bool _contentOfNoteIsTapped = true;
        public bool ContentOfNoteIsTapped
        {
            get { return _contentOfNoteIsTapped; }
            set
            {
                _contentOfNoteIsTapped = value;
                OnPropertyChanged();
            }
        }


        private bool _contentOfNoteIsReadOnly = true;

        public bool ContentOfNoteIsReadOnly
        {
            get { return _contentOfNoteIsReadOnly; }
            set
            {
                _contentOfNoteIsReadOnly = value;
                OnPropertyChanged();
            }
        }

        private string _oldContent = "";



        delegate void ToShareView(object sender, ItemClickEventArgs e);
        private void UsersToShareView_ItemClick(object sender, ItemClickEventArgs e)
        {
            _noteContentViewModel = NoteContentVM.Singleton;
            UWPYourNoteLibrary.Models.User selectedUser = (UWPYourNoteLibrary.Models.User)e.ClickedItem;
            _noteContentViewModel.ShareNote(selectedUser.userId, _noteId);

        }



        //----------------------------------------Auto Save----------------------------------------
        public void TitleOfNoteTapped(object sender, TappedRoutedEventArgs e)
        {
           if(TitleOfNoteIsTapped)
            EditModeEnabled();
        }

        public void ContentOfNoteTapped(object sender, TappedRoutedEventArgs e)
        {
           if(ContentOfNoteIsTapped)
            EditModeEnabled();
        }

        public void EditModeEnabled()
        {
            if (NoteMenuOptionsVisibility == Visibility.Collapsed)
                NoteMenuOptionsVisibility = Visibility.Visible;
            if (TitleOfNoteIsReadOnly == true || ContentOfNoteIsReadOnly == true)
            {
                TitleOfNoteIsReadOnly = false;
                ContentOfNoteIsReadOnly = false;

                TitleOfNoteIsTapped = false;
                ContentOfNoteIsTapped = false;
                NoteContentIsTapped = false;

                _oldContent = ContentOfNoteText;
                _oldTitle = TitleOfNoteText;
                _dispatcherTimer = new DispatcherTimer();
                DispatcherTimerStart(_dispatcherTimer);
            }
        }
        private void DispatcherTimerStart(DispatcherTimer dispatcherTimer)
        {
            if(dispatcherTimer !=null)
            {
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
                dispatcherTimer.Start();
            }
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
            string modifiedDay = DateTime.Now.ToString("MMM/dd/yyyy hh:mm:ss.fff tt");
            Note updateNote = new Note(_noteId, TitleOfNoteText, ContentOfNoteText, modifiedDay);

            noteContentVM = NoteContentVM.Singleton;
            noteContentVM.noteContentView = this;

            noteContentVM.UpdateNote(updateNote, titleChange, contentChange);
            if (contentChange && titleChange)
            {
                _oldContent = ContentOfNoteText;
                _oldTitle = TitleOfNoteText;
                isModified = true;
            }
            else
            {
                if (contentChange)
                {
                    _oldContent = ContentOfNoteText;
                    isModified = true;
                }
                if (titleChange)
                {
                    _oldTitle = TitleOfNoteText;
                    isModified = true;
                }
            }
           
        }

        private bool IsChanged(string oContext, string nContext)
        {
            bool isChange = false;
            if (oContext != nContext)
                isChange = true;
            return isChange;
        }

      

        private void ContentOfNoteTextChanged(object sender, RoutedEventArgs e)
        {
            RichEditBox Context = (RichEditBox)sender;
            string text;

                Context.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf,out text)
      ;
            ContentOfNoteText = text;

        }

        private void TitleOfNoteTextChanged(object sender, RoutedEventArgs e)
        {

            TextBox box = (TextBox)sender;
            TitleOfNoteText = box.Text;
           

        }
        public long GetNoteColor()
        {
          
            return NoteMenuOptions.ColorOptionsSelectedIndex;
        }
        private void NoteBackgroundColor()
        {
            NoteContentBackground = NoteMenuOptions.NoteColorForeground;
           
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
                        if ("True" == name)
                            NoteShared(true);
                        else
                            NoteShared(false);
                        break;

                    }
                default: return;
            }
        }


    }
}
