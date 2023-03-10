using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace YourNoteUWP.Util
{
    internal class NotesUtilities
    {

        public static Dictionary<int, string> noteColorStyle = new Dictionary<int, string>()
        {
            {0, "RedNoteColor" },
              {1, "GreenNoteColor" },
                {2, "YellowNoteColor" },
                  {3, "BlueNoteColor" }

        };
        public static Dictionary<int, string> noteColorButtonStyle = new Dictionary<int, string>()
        {
            {0, "RedColorNoteButton" },
              {1, "GreenColorNoteButton" },
                {2, "YellowColorNoteButton" },
                  {3, "BlueColorNoteButton" }

        };


        public static SolidColorBrush GetSolidColorBrush(long value)
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

   

        //----Note Font Background

            public static void FontBackgroundClick(object sender, RoutedEventArgs e)
        {
            RichEditBox box = (RichEditBox)sender;
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.BackgroundColor.R == 0 && charFormatting.BackgroundColor.G == 0 && charFormatting.BackgroundColor.B == 0)
                {
                    charFormatting.BackgroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255);
                    charFormatting.ForegroundColor = Windows.UI.Color.FromArgb(0, 0, 0, 0);
                }
                else
                {
                    charFormatting.BackgroundColor = Windows.UI.Color.FromArgb(0, 0, 0, 0);
                    charFormatting.ForegroundColor = Windows.UI.Color.FromArgb(0, 255, 255, 255);
                }

            }
        }

        //----Note Font Increase
        public static  void FontIncreaseFunc(Windows.UI.Text.ITextSelection selectedText, float limit, float value)
        {


            Windows.UI.Text.ITextCharacterFormat charFormatting;
            if (selectedText != null)
            {
                charFormatting = selectedText.CharacterFormat;
                string text = selectedText.Text;
                float size = charFormatting.Size;
                if (!String.IsNullOrEmpty(text) && size < limit)
                {
                    charFormatting.Size += value;
                }
            }
        }
        public static void FontIncreaseClick(object sender, RoutedEventArgs e)
        {
            RichEditBox box = (RichEditBox)sender;

            FontIncreaseFunc(box.Document.Selection, (float)10.5, (float)0.5);
        }

        //----Note Font Decrease
        public static void FontDecreaseFunc(Windows.UI.Text.ITextSelection selectedText, float limit, float value)
        {


            Windows.UI.Text.ITextCharacterFormat charFormatting;
            if (selectedText != null)
            {
                charFormatting = selectedText.CharacterFormat;
                string text = selectedText.Text;
                float size = charFormatting.Size;
                if (!String.IsNullOrEmpty(text) && size > limit)
                {
                    charFormatting.Size -= value;
                }
            }
        }

        public static void FontDecreaseClick(object sender, RoutedEventArgs e)
        {

            RichEditBox box = (RichEditBox)sender;

            FontDecreaseFunc(box.Document.Selection, (float)7.5, (float)0.5);
        }

        //----Note Small Caps
        public static void SmallCapsClick(object sender, RoutedEventArgs e)
        {

            RichEditBox box = (RichEditBox)sender;

            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.SmallCaps = Windows.UI.Text.FormatEffect.Toggle;
            }

        }

        //----Note All Caps
        public static void AllCapsClick(object sender, RoutedEventArgs e)
        {
            RichEditBox box = (RichEditBox)sender;


            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.AllCaps = Windows.UI.Text.FormatEffect.Toggle;
            }

        }

        //----Note Strikethrough
        public static void StrikethroughClick(object sender, RoutedEventArgs e)
        {
            RichEditBox box = (RichEditBox)sender;


            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Strikethrough = Windows.UI.Text.FormatEffect.Toggle;
            }
        }
    }
}
