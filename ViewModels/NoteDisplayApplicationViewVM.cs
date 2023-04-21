using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPYourNote.ViewModels;
using UWPYourNoteLibrary.Data.Managers;
using UWPYourNoteLibrary.Domain.Contract;
using UWPYourNoteLibrary.Domain.UseCase;
using UWPYourNoteLibrary.Models;
using UWPYourNoteLibrary.Util;

namespace UWPYourNote.ViewModels
{
    public class NoteDisplayApplicationViewVM
    {

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
        private class UpdateNotePresenterCallBack : ICallback<UpdateNoteUseCaseResponse>
        {
            private NoteDisplayApplicationViewVM Presenter;
            public UpdateNotePresenterCallBack(NoteDisplayApplicationViewVM presenter)
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

        private class ShareNotePresenterCallBack : ICallback<ShareNoteUseCaseResponse>
        {
            private NoteDisplayApplicationViewVM Presenter;
            public ShareNotePresenterCallBack(NoteDisplayApplicationViewVM presenter)
            {
                Presenter = presenter;
            }

            public void onFailure(ShareNoteUseCaseResponse response)
            {
               // Presenter?.IsNoteShared(response.Result);
            }

            public void onSuccess(ShareNoteUseCaseResponse response)
            {
               // Presenter?.IsNoteShared(response.Result);

            }
        }


        private class DeleteNotePresenterCallBack : ICallback<DeleteNoteUseCaseResponse>
        {
            private NoteDisplayApplicationViewVM Presenter;
            public DeleteNotePresenterCallBack(NoteDisplayApplicationViewVM presenter)
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

        private class UpdateCountPresenterCallBack : ICallback<UpdateCountUseCaseResponse>
        {
            NoteDisplayApplicationViewVM Presenter;
            string value = null;
            public UpdateCountPresenterCallBack(NoteDisplayApplicationViewVM presenter)
            {
                Presenter = presenter;
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
            NoteDisplayApplicationViewVM Presenter;
            string value = null;
            public UpdateNoteColorPresenterCallBack(NoteDisplayApplicationViewVM presenter)
            {
                Presenter = presenter;
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
            NoteDisplayApplicationViewVM Presenter;
            string value = null;
            public CanShareNotePresenterCallBack(NoteDisplayApplicationViewVM presenter)
            {
                Presenter = presenter;
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
            NoteDisplayApplicationViewVM Presenter;
            string value = null;
            public ValidUsersToSharePresenterCallBack(NoteDisplayApplicationViewVM presenter)
            {
                Presenter = presenter;
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
