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

namespace UWPYourNote.ViewModels
{
    public class NoteDisplayApplicationViewVM
    {

        public void UpdateNote(Note noteToUpdate, bool titleChange, bool contentChange)
        {
            UpdateNoteUseCaseRequest request = new UpdateNoteUseCaseRequest();
            request.NoteToUpdate = noteToUpdate;
            request.IsTitleChanged = titleChange;
            request.IsContentChanged = contentChange;
            UpdateNoteUseCase usecase = new UpdateNoteUseCase(request, new UpdateNotePresenterCallBack(this));
            usecase.Action();

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
    }
}
