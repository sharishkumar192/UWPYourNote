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
using UWPYourNote.View.usercontrol;

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
            NotesUtilities.UpdateNote(noteToUpdate, titleChange, contentChange, new UpdateNotePresenterCallBack(this));
        }







        public void DeleteNote(long noteId)
        {
            NotesUtilities.DeleteNote(noteId, new DeleteNotePresenterCallBack(this));
        }



        public void ShareNote(string sharedUserId, long noteId)
        {
            NotesUtilities.ShareNote(sharedUserId, noteId, new ShareNotePresenterCallBack(this));

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


        public void GetUsersToShare(string userId, long displayNoteId)
        {
            NotesUtilities.ValidUsersToShare(userId, displayNoteId, new ValidUsersToSharePresenterCallBack(this));

        }

        public void IsOwner(string userId, long noteId)
        {
            NotesUtilities.CanShareNote(userId, noteId, new CanShareNotePresenterCallBack(this));

        }
        public void UpdateNoteColor(long noteId, long noteColor, string modifiedDay)
        {
            NotesUtilities.UpdateNoteColor(noteId, noteColor, modifiedDay, new UpdateNoteColorPresenterCallBack(this));
        }

        public void UpdateCount(long searchCount, long noteId)
        {
            NotesUtilities.UpdateCount(searchCount, noteId, new UpdateCountPresenterCallBack(this));
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


        private class UpdateCountPresenterCallBack : ICallback<UpdateCountUseCaseResponse>
        {
            NoteContentVM Presenter;
            string value = null;
            public UpdateCountPresenterCallBack(NoteContentVM noteContentVM)
            {
                Presenter = noteContentVM;
            }


            public void onSuccess(UpdateCountUseCaseResponse result)
            {
            }

            public void onFailure(UpdateCountUseCaseResponse result)
            {

            }
        }

        private class UpdateNoteColorPresenterCallBack : ICallback<UpdateNoteColorUseCaseResponse>
        {
            NoteContentVM Presenter;
            string value = null;
            public UpdateNoteColorPresenterCallBack(NoteContentVM noteContentVM)
            {
                Presenter = noteContentVM;
            }


            public void onSuccess(UpdateNoteColorUseCaseResponse result)
            {

            }

            public void onFailure(UpdateNoteColorUseCaseResponse result)
            {

            }
        }



        private class CanShareNotePresenterCallBack : ICallback<CanShareNoteUseCaseResponse>
        {
            NoteContentVM Presenter;
            string value = null;
            public CanShareNotePresenterCallBack(NoteContentVM noteContentVM)
            {
                Presenter = noteContentVM;
            }


            public void onSuccess(CanShareNoteUseCaseResponse result)
            {

            }

            public void onFailure(CanShareNoteUseCaseResponse result)
            {

            }
        }


        private class ValidUsersToSharePresenterCallBack : ICallback<ValidUsersToShareUseCaseResponse>
        {
            NoteContentVM Presenter;
            string value = null;
            public ValidUsersToSharePresenterCallBack(NoteContentVM noteContentVM)
            {
                Presenter = noteContentVM;
            }


            public void onSuccess(ValidUsersToShareUseCaseResponse result)
            {

            }

            public void onFailure(ValidUsersToShareUseCaseResponse result)
            {

            }
        }






    }
}
