using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YourNoteUWP.ViewModels.Contract
{
    public interface INoteContentView
    {
        void CanShareNote(bool result);
        void ValidUsersLists(ObservableCollection<UWPYourNoteLibrary.Models.User> listOfUsers);
    }
}
