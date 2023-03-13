using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
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
using static System.Net.Mime.MediaTypeNames;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPYourNote
{
    public sealed partial class NotesDataTemplate : UserControl, INotifyPropertyChanged
    {
        // private static bool loadOnce = false;
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static string NotePreview(string title, string content)
        {
            string text = "";
            if (title != "")
            {
                text = text + title + "\r\n";


            }
            if (content != "")
            {
                text += content;
            }
            if (title == "")
                text = "(Empty Note)";
            return text;
        }


        private SolidColorBrush _noteContentBackground;

        public SolidColorBrush NoteContentBackground
        {
            get { return _noteContentBackground; }
            set
            {
                _noteContentBackground = value;
                OnPropertyChanged();
            }
        }



        public string ShowModifiedTime(string modifiedTime)
        {
            Contents.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, notesTemplate.content);

            Contents.IsEnabled = false;

            string value = "";
            string currentStatus = DateTime.Now.ToString("MMM/dd/yyyy hh:mm:ss.fff tt");
            DateTime currentDetail = DateTime.Parse(currentStatus);

            DateTime modifiedDetail = DateTime.Parse(modifiedTime);

            if (currentDetail.Year == modifiedDetail.Year)
            {
                if (currentDetail.Month == modifiedDetail.Month)
                {
                    if (currentDetail.Day == modifiedDetail.Day)
                    {
                        int minutes = Math.Abs((currentDetail.Hour * 60 + currentDetail.Minute) - (modifiedDetail.Hour * 60 + modifiedDetail.Minute));
                        if (minutes == 0)
                            value = "just now";
                        else if (minutes < 60)
                        {
                            value = minutes.ToString() + " " + "minutes ago";
                        }
                        else
                        {
                            value = modifiedDetail.ToString("hh:mm tt");

                        }
                    }
                    else
                    {
                        string date = modifiedDetail.DayOfWeek.ToString();
                        value = date.Substring(0, 3);
                        value += " " + modifiedDetail.ToString("MMM") + " " + modifiedDetail.ToString("dd");
                    }
                }
                else
                {
                    string date = modifiedDetail.DayOfWeek.ToString();
                    value = date.Substring(0, 3);
                    value += " " + modifiedDetail.ToString("MMM") + " " + modifiedDetail.ToString("dd");
                }
            }
            else
            {
                value = modifiedDetail.ToString("MMM") + " " + modifiedDetail.ToString("dd") + ", " + modifiedDetail.Year;
            }


            return value;
        }
        public void SetColor()
        {
            if (this.DataContext != null)
                NoteContentBackground = GetSolidColorBrush(notesTemplate.noteColor);
        }

        public UWPYourNoteLibrary.Models.Note notesTemplate
        {


            get
            {
                // loadOnce = true;
                // SetColor();
                return this.DataContext as UWPYourNoteLibrary.Models.Note;



            }
        }


        public NotesDataTemplate()
        {
            this.InitializeComponent();

            //if (notesTemplate != null)
            //{
            //          
            //}




            this.DataContextChanged += NotesDataTemplate_DataContextChanged;
            //if (loadOnce == true)
            //{

            //   
            //}


        }

        private void NotesDataTemplate_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (notesTemplate != null)
                NoteContentBackground = GetSolidColorBrush(notesTemplate.noteColor);
            Bindings.Update();
        }

        private static SolidColorBrush GetSolidColorBrush(long value)
        {
            int index = (int)value;
            List<string> color = new List<string>()
        { "#f8bec5", "#c6e8b7", "#fdefad", "#c3e9fd"};
            string hex = color[index];
            hex = hex.Replace("#", string.Empty);
            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb((byte)255, r, g, b));
            return myBrush;
        }
    }
}
