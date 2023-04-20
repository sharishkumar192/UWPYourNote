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
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPYourNote.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoteDisplayApplicationView : Page, INotifyPropertyChanged
    {
        public DispatcherTimer _dispatcherTimer = null;
        private string _oldContent = "";
        private string _oldTitle = "";
          public bool isModified = false;
        public bool isDeleted = false;
        private Frame _frame = null;
        public NoteDisplayApplicationViewVM noteDisplayApplicationVM;
        public NoteDisplayApplicationView()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }


        private Note _displayNote = null ;

        public Note DisplayNote
        {
            get { return _displayNote; }
            set { _displayNote = value;
                OnPropertyChanged();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            DisplayNote = (Note)e.Parameter;
            NoteContentBackground = NotesUtilities.GetSolidColorBrush(DisplayNote.noteColor);
            TitleOfNoteText  = _oldTitle = DisplayNote.title;
            ContentOfNoteText  = _oldContent  = DisplayNote.content;
            ContentOfNote.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, ContentOfNoteText);
            _dispatcherTimer = new DispatcherTimer();
        
            DispatcherTimerStart(_dispatcherTimer);
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


      
        public void DispatcherTimerStart(DispatcherTimer dispatcherTimer)
        {
            if (dispatcherTimer != null)
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
            Note updateNote = new Note(DisplayNote.noteId, TitleOfNoteText, ContentOfNoteText, modifiedDay);
            noteDisplayApplicationVM = new NoteDisplayApplicationViewVM();
            // noteDisplayApplicationVM.noteContentView = this;

            noteDisplayApplicationVM.UpdateNote(updateNote, titleChange, contentChange);
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

        private void TitleOfNoteTextChanged(object sender, RoutedEventArgs e)
        {

            TextBox box = (TextBox)sender;
            TitleOfNoteText = box.Text;


        }

    


        private void ToEnableEditMode()
        {
          
            _dispatcherTimer = null;
            isModified = false;
            NoteMenuOptions.UsersToShare = null;
            isDeleted = false;



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
                case "MinimizeButton": MinimizeButton(); break;
                case "PopOutButton": PopOutButton(); break;
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
       //     NoteContentBackground = NoteMenuOptions.NoteColorForeground;

        }

        private void MinimizeButton()
        {
            _frame.Content = null;
        }

        private async void PopOutButton()
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                Tuple<Note, Frame> tp = new Tuple<Note, Frame>(DisplayNote, null);
                frame.Navigate(typeof(NoteDisplayApplicationView), tp);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId, ViewSizePreference.UseLess);
            _frame.Content = null;

        }

        //----------------------------Note Delete Button ---------------------------------------------------
        public void NoteDeleteButtonClick(object sender, RoutedEventArgs e)
        {

           // noteContentVM = NoteContentVM.Singleton;
        //    noteContentVM.DeleteNote(_noteId);
            isDeleted = true;
          //  _delPageMethod.DynamicInvoke(null, null);
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

      
    }



}
