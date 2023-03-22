using UWPYourNoteLibrary.Models;
using System.Collections.ObjectModel;
using YourNoteUWP.ViewModels.Contract;
using UWPYourNoteLibrary.Domain.Contract;
using UWPYourNoteLibrary.Domain.UseCase;
using Windows.System;

namespace UWPYourNote.ViewModels
{
    internal class NoteContentVM  
    {

        private NoteContentVM()
        {

        }

        private static NoteContentVM _noteViewModel;

        public static NoteContentVM Singleton
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


        public INoteContentView noteContentView;


        public void UpdateNote(Note noteToUpdate, bool titleChange, bool contentChange)
        {
            UpdateNoteUseCaseRequest request = new UpdateNoteUseCaseRequest();
            request.NoteToUpdate = noteToUpdate;
            request.IsTitleChanged = titleChange;
            request.IsContentChanged = contentChange;
            UpdateNoteUseCase usecase = new UpdateNoteUseCase(request, new UpdateNotePresenterCallBack(this));
            usecase.Action();

        }

        public void UpdateCount(long searchCount, long noteId)
        {
          DBUpdation.UpdateNoteCount(DBCreation.notesTableName, searchCount, noteId);
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




        //-----------------------------------------PRESENTER CALLBACK-----------------------------------------------------------

        private class UpdateNotePresenterCallBack : ICallback<UpdateNoteUseCaseResponse>
        {
            private NoteContentVM Presenter;
            public UpdateNotePresenterCallBack(NoteContentVM presenter)
            {
                Presenter = presenter;
            }

            public void onFailure(UpdateNoteUseCaseResponse result)
            {
            }

            public void onSuccess(UpdateNoteUseCaseResponse result)
            {

          
            }
        }



    }
}
