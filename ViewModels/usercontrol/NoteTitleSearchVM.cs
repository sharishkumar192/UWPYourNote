using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UWPYourNote.ViewModels;
using UWPYourNote.ViewModels.Contract;
using UWPYourNoteLibrary.Domain.Contract;
using UWPYourNoteLibrary.Domain.UseCase;
using UWPYourNoteLibrary.Models;
using Windows.UI.Xaml.Controls;
using YourNoteUWP.ViewModels.Contract;

namespace UWPYourNote.ViewModels.usercontrol
{
    public class NoteTitleSearchVM
    {
        private static NoteTitleSearchVM noteTitleSearchVM;
        public static NoteTitleSearchVM Singleton
        {
            get
            {
                if (noteTitleSearchVM == null)
                    noteTitleSearchVM = new NoteTitleSearchVM();
                return noteTitleSearchVM;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public INoteTitleSearchBox noteTitleSearchView { get; set; }
        //--------------------------------------------- SEARCH NOTES BASED ON THE TEXT/ RECENT SEARCHES-----------------------------------

        void AssignSuggestedNotes(ObservableCollection<Note> notes)
         {
            UserControl page = (UserControl)noteTitleSearchView;
            _ = page?.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                noteTitleSearchVM.SuggestionContentItemSource.Clear();
                if (notes == null || notes.Count == 0) { return; }
                foreach (Note note in notes)
                {
                    noteTitleSearchVM.SuggestionContentItemSource.Add(note);
                }
                // noteTitleSearchVM.SuggestionContentItemSource = notes;


            });
        }



        private ObservableCollection<UWPYourNoteLibrary.Models.Note> _suggestionContentItemSource = new ObservableCollection<Note>();
        public ObservableCollection<UWPYourNoteLibrary.Models.Note> SuggestionContentItemSource
        {
            get { return _suggestionContentItemSource; }
            set
            {
                _suggestionContentItemSource = value;

                OnPropertyChanged();

            }
        }

        public void GetSuggestedAndRecentNotes(string userId, string text)
        {
            GetSuggestedAndRecentNotesUseCaseRequest request = new GetSuggestedAndRecentNotesUseCaseRequest();
            request.UserId = userId;
            request.Text = text;
            GetSuggestedAndRecentNotesUseCase usecase = new GetSuggestedAndRecentNotesUseCase(request, new GetSuggestedAndRecentNotesPresenterCallBack(this));
            usecase.Execute();
        }


        private class GetSuggestedAndRecentNotesPresenterCallBack : ICallback<GetSuggestedAndRecentNotesUseCaseResponse>
        {
            private NoteTitleSearchVM Presenter;
            public GetSuggestedAndRecentNotesPresenterCallBack(NoteTitleSearchVM presenter)
            {
                Presenter = presenter;
            }

            public void onFailure(GetSuggestedAndRecentNotesUseCaseResponse result)
            {

                Presenter?.AssignSuggestedNotes(result.List);


            }

            public void onSuccess(GetSuggestedAndRecentNotesUseCaseResponse result)
            {
                Presenter?.AssignSuggestedNotes(result.List);

            }
        }

    }
}
