using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPYourNote.ViewModels
{
    internal class MainPageViewModel 
    {
       
       static Frame _mainFrame;
        public MainPageViewModel(Frame frame)
        {
            _mainFrame = frame;

        }
        public MainPageViewModel()
        {
            //_mainFrame = frame;

        }

    
    }
}
