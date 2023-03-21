using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPYourNoteLibrary.Models;
namespace UWPYourNote.ViewModels.Contract
{ 
    internal interface ISignInPageView
    {
        void NavigateToOnSuccess(User user);
        void NavigateToOnFailure();
    }
}
