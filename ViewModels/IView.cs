using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace YourNoteUWP.ViewModels
{
    internal interface IView
    {
        void NavigateTo();
    }
}
