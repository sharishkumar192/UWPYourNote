using UWPYourNoteLibrary.Models;
using System.Collections.ObjectModel;
namespace UWPYourNote.ViewModels
{
    internal class NoteContentVM  
    {

        private NoteContentVM()
        {

        }

        private static NoteContentVM _noteViewModel;

        public static NoteContentVM NoteViewModel
        {
            get
            {
                if (_noteViewModel == null)
                {
                    _noteViewModel = new NoteContentVM();
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


        public ObservableCollection<UWPYourNoteLibrary.Models.User> GetUsersToShare(string userId, long displayNoteId)
        {
            return DBFetch.ValidUsersToShare(DBCreation.userTableName, DBCreation.sharedTableName, DBCreation.notesTableName, userId, displayNoteId);
        }

        public void ChangeNoteColor(long noteId, long noteColor, string modifiedDay)
        {
            DBUpdation.UpdateNoteColor(DBCreation.notesTableName, noteId, noteColor, modifiedDay);

        }



    }
}
