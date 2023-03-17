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
    internal class MainPageVM 
    {
       
       static Frame _mainFrame;
        public MainPageVM(Frame frame)
        {
            _mainFrame = frame;

        }
        public MainPageVM()
        {
            //_mainFrame = frame;

        }

    
    }
}
