using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using YourNoteUWP.Models;
using System.Runtime.CompilerServices;
using System.Data.SQLite;
using Windows.Storage;
using System.Collections.ObjectModel;

namespace YourNoteUWP.ViewModels
{
    internal class NoteContentViewModel  
    {

        private NoteContentViewModel()
        {

        }

        private static NoteContentViewModel _noteViewModel;

        public static NoteContentViewModel NoteViewModel
        {
            get
            {
                if (_noteViewModel == null)
                {
                    _noteViewModel = new NoteContentViewModel();
                }
                return _noteViewModel;
            }
        }


    


        public void UpdateCount(long searchCount, long noteId)
        {
            DBUpdation.UpdateNoteCount(DBCreation.notesTableName, searchCount, noteId);
        }
        public  void NoteContentUpdation(string content, long noteId, string modifiedDay)
        {
            DBUpdation.UpdateNoteContent(DBCreation.notesTableName, content, noteId, modifiedDay);
        }

        public void NoteUpdation(string title, string content, long noteId, string modifiedDay)
        {
            DBUpdation.UpdateNote(DBCreation.notesTableName, title, content, noteId, modifiedDay);
        }
        public void NoteTitleUpdation(string title, long noteId, string modifiedDay)
        {
            DBUpdation.UpdateNoteTitle(DBCreation.notesTableName, title, noteId, modifiedDay);
        }

        public  void DeleteNote(long noteId)
        {
            DBUpdation.DeleteNote(DBCreation.notesTableName, DBCreation.sharedTableName, noteId);
        }


        public bool IsOwner(string userId, long noteId)
        {
            return DBFetch.CanShareNote(DBCreation.notesTableName, userId, noteId);
        }


        public void ShareNote(string sharedUserId, long noteId)
        {
            DBUpdation.InsertSharedNote(DBCreation.sharedTableName, sharedUserId, noteId);
        }


        public ObservableCollection<Models.User> GetUsersToShare(string userId, long displayNoteId)
        {
            return DBFetch.ValidUsersToShare(DBCreation.userTableName, DBCreation.sharedTableName, DBCreation.notesTableName, userId, displayNoteId);
        }

        public void ChangeNoteColor(long noteId, long noteColor, string modifiedDay)
        {
            DBUpdation.UpdateNoteColor(DBCreation.notesTableName, noteId, noteColor, modifiedDay);

        }



    }
}
