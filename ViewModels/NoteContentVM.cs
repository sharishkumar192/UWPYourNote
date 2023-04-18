using UWPYourNoteLibrary.Models;
using System.Collections.ObjectModel;
using YourNoteUWP.ViewModels.Contract;
using UWPYourNoteLibrary.Domain.Contract;
using UWPYourNoteLibrary.Domain.UseCase;
using Windows.System;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Popups;
using System;
using UWPYourNote.ViewModels.Contract;
using Windows.UI.Xaml.Controls;
using UWPYourNoteLibrary.Util;
using System.Diagnostics;

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
          DBUpdation.UpdateNoteCount(NotesUtilities.notesTableName, searchCount, noteId);
        }


   

        public void DeleteNote(long noteId)
        {
            DeleteNoteUseCaseRequest request = new DeleteNoteUseCaseRequest();
            request.NoteId = noteId;
            DeleteNoteUseCase usecase = new DeleteNoteUseCase(request, new DeleteNotePresenterCallBack(this));
            usecase.Action();
        }
        public bool IsOwner(string userId, long noteId)
        {
            return DBFetch.CanShareNote(NotesUtilities.notesTableName, userId, noteId);
        }


        public void ShareNote(string sharedUserId, long noteId)
        {
            ShareNoteUseCaseRequest request = new ShareNoteUseCaseRequest();
            request.NoteId = noteId;
            request.SharedUserID = sharedUserId;
            ShareNoteUseCase usecase = new ShareNoteUseCase(request, new ShareNotePresenterCallBack(this));
            usecase.Action();

          
        }
        public async void IsNoteShared(bool value)
        {
            MessageDialog showDialog;
            if (value == true)
                showDialog = new MessageDialog("Note has been shared!");
            else
                showDialog = new MessageDialog("You cant share this note, as your not the owner!");
            showDialog.Commands.Add(new UICommand("Ok")
            {
                Id = 0
            });
            showDialog.DefaultCommandIndex = 0;
            var result = await showDialog.ShowAsync();
        }


        public ObservableCollection<UWPYourNoteLibrary.Models.User> GetUsersToShare(string userId, long displayNoteId)
        {
            return DBFetch.ValidUsersToShare(UserUtilities.userTableName, NotesUtilities.sharedTableName, NotesUtilities.notesTableName, userId, displayNoteId);
        }

        public void ChangeNoteColor(long noteId, long noteColor, string modifiedDay)
        {
            DBUpdation.UpdateNoteColor(NotesUtilities.notesTableName, noteId, noteColor, modifiedDay);

        }




        //-----------------------------------------PRESENTER CALLBACK-----------------------------------------------------------

        private class UpdateNotePresenterCallBack : ICallback<UpdateNoteUseCaseResponse>
        {
            Stopwatch stopwatch = new Stopwatch();
            private NoteContentVM Presenter;
            public UpdateNotePresenterCallBack(NoteContentVM presenter)
            {
                stopwatch.Start();
             
                Presenter = presenter;
            }

            public void onFailure(UpdateNoteUseCaseResponse result)
            {
                stopwatch.Stop();
                Debug.WriteLine(stopwatch.ElapsedMilliseconds);
                stopwatch = null;
            }

            public void onSuccess(UpdateNoteUseCaseResponse result)
            {
                stopwatch.Stop();
            
                Debug.WriteLine(stopwatch.ElapsedMilliseconds);
                stopwatch = null;
            }
        }

        private class ShareNotePresenterCallBack : ICallback<ShareNoteUseCaseResponse>
        {
            private NoteContentVM Presenter;
            public ShareNotePresenterCallBack(NoteContentVM presenter)
            {
                Presenter = presenter;
            }

            public void onFailure(ShareNoteUseCaseResponse response)
            {
                Presenter?.IsNoteShared(response.Result);
            }

            public void onSuccess(ShareNoteUseCaseResponse response)
            {
                Presenter?.IsNoteShared(response.Result);

            }
        }


        private class DeleteNotePresenterCallBack : ICallback<DeleteNoteUseCaseResponse>
        {
            private NoteContentVM Presenter;
            public DeleteNotePresenterCallBack(NoteContentVM presenter)
            {
                Presenter = presenter;
            }

            public void onFailure(DeleteNoteUseCaseResponse response)
            {
               
            }

            public void onSuccess(DeleteNoteUseCaseResponse response)
            {
                

            }
        }



    }
}
