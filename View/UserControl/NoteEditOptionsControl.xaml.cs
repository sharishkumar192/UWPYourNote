using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
using UWPYourNoteLibrary.Util;
using UWPYourNote.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPYourNote.View.usercontrol
{
    public sealed partial class NoteEditOptionsControl : UserControl, INotifyPropertyChanged
    {
        public event Action<string> EditOptions;

        public NoteEditOptionsControl()
        {
            this.InitializeComponent();
        }

     


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Delegate _delPageMethod;
        public Delegate CallingPageMethod
        {
            set { _delPageMethod = value; }
        }

        private Delegate _toShare;
        public Delegate ToShare
        {
            set { _toShare = value; }
        }



        string ReturnNameOfButton(object sender)
        {
            Button button = sender as Button;
            return button.Name;
        }
       
        public SolidColorBrush ChangeDataSeparatorColor(string name)
        {
            return NotesUtilities.GetSolidColorBrush(NotesUtilities.noteShareBackgroundStyle[ColorOptionsSelectedIndex]);
        }

        //private string ReturnNameOfButton(object sender)
        //{
        //    Button button = (Button)sender;
        //    return button.Name;
        //}

        //----Note Font Background

        private void FontBackgroundClick(object sender, RoutedEventArgs e)
        {

            EditOptions?.Invoke(ReturnNameOfButton(sender));
        }

        //----Note Font Increase
        private void FontIncreaseClick(object sender, RoutedEventArgs e)
        {
            EditOptions?.Invoke(ReturnNameOfButton(sender));
        }

        //----Note Font Decrease

        private void FontDecreaseClick(object sender, RoutedEventArgs e)
        {
            EditOptions?.Invoke(ReturnNameOfButton(sender));
        }

        //----Note Small Caps
        private void SmallCapsClick(object sender, RoutedEventArgs e)
        {
            EditOptions?.Invoke(ReturnNameOfButton(sender));
        }

        //----Note All Caps
        private void AllCapsClick(object sender, RoutedEventArgs e)
        {
            EditOptions?.Invoke(ReturnNameOfButton(sender));
        }

        //----Note Strikethrough
        private void StrikethroughClick(object sender, RoutedEventArgs e)
        {
            EditOptions?.Invoke(ReturnNameOfButton(sender));
        }

        //----Note Color Button

        private SolidColorBrush _noteColorForeground;
        public SolidColorBrush NoteColorForeground
        {
            get { return _noteColorForeground; }
            set
            {
                _noteColorForeground = value;
                OnPropertyChanged();
            }
        }

        private void ChangeNoteColor()
        {
         //   if (NoteShareButtonVisibility == Visibility.Visible && NoteDeleteButtonVisibility == Visibility.Visible)
                Style style1 = Application.Current.Resources[UWPYourNoteLibrary.Util.NotesUtilities.noteColorStyle[ColorOptionsSelectedIndex]] as Style;
                Style style2 = Application.Current.Resources[UWPYourNoteLibrary.Util.NotesUtilities.noteColorButtonStyle[ColorOptionsSelectedIndex]] as Style;
              
                FontBackground.Style = FontIncrease.Style = FontDecrease.Style = SmallCaps.Style = AllCaps.Style = Strikethrough.Style = NoteShareButton.Style = style1;
                NoteColor.Style = NoteDeleteButton.Style = style2;

            UsersToShareView.Background = NotesUtilities.GetSolidColorBrush(NotesUtilities.noteShareBackgroundStyle[ColorOptionsSelectedIndex]);
            // NotesUtilities.NoteBackgroundColor(ColorOptionsSelectedIndex);
            // SolidColorBrush bgColor = NotesUtilities.GetSolidColorBrush(NotesUtilities.noteShareBackgroundStyle[ColorOptionsSelectedIndex]);

            //   UsersToShareViewBackground = bgColor;

        }


        private int _colorOptionsSelectedIndex = 0;

        public int ColorOptionsSelectedIndex
        {
            get { return _colorOptionsSelectedIndex; }
            set
            {
                _colorOptionsSelectedIndex = value;
                OnPropertyChanged();
                ChangeNoteColor();
            }
        }


        private void ColorOptionsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox box = (ListBox)sender;

            ColorOptionsSelectedIndex = box.SelectedIndex;
            NoteColorForeground = UWPYourNoteLibrary.Util.NotesUtilities.GetSolidColorBrush(box.SelectedIndex);

            EditOptions?.Invoke(box.Name);
            ChangeNoteColor();

        }


        public void NoteDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            EditOptions?.Invoke(ReturnNameOfButton(sender));
        }
        public void NoteShareButtonClick(object sender, RoutedEventArgs e)
        {
            if (UsersToShare == null)
            {
                UsersToShare = (ObservableCollection<UWPYourNoteLibrary.Models.User>)_delPageMethod.DynamicInvoke(null, null);
            }

            if (UsersToShare != null && UsersToShare.Count == 0)
                NoValidUsers();
        }

        private void NoValidUsers()
        {
            EditOptions?.Invoke("NoValidUsers");
        }

        //private SolidColorBrush _usersToShareViewBackground;// = NotesUtilities.GetSolidColorBrush(NotesUtilities.noteShareBackgroundStyle[0]);

        //public SolidColorBrush UsersToShareViewBackground
        //{
        //    get { return _usersToShareViewBackground; }
        //    set { _usersToShareViewBackground = value; }
        //}



        private void UsersToShareView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListView view = (ListView)sender;
            _toShare.DynamicInvoke(view.Name, e);

            int i = UsersToShare.IndexOf((UWPYourNoteLibrary.Models.User)e.ClickedItem);
            UsersToShare.RemoveAt(i);

            NoteShared(true);
            //EditOptions?.Invoke(view.name,e);
        }

        private void NoteShared(bool value)
        {
            EditOptions?.Invoke(value.ToString());
        }

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

        private Visibility _noteShareButtonVisibility;

        public Visibility NoteShareButtonVisibility
        {
            get { return _noteShareButtonVisibility; }
            set { _noteShareButtonVisibility = value; }
        }

        private Visibility _noteDeleteButtonVisibility;

        public Visibility NoteDeleteButtonVisibility
        {
            get { return _noteDeleteButtonVisibility; }
            set { _noteDeleteButtonVisibility = value; }
        }


    }
}
