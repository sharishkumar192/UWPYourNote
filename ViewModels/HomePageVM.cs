using Microsoft.Toolkit.Uwp.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using UWPYourNote.ViewModels.Contract;
using UWPYourNoteLibrary.Models;
using UWPYourNoteLibrary.Domain.Contract;
using UWPYourNoteLibrary.Domain.UseCase;
using Windows.UI.Xaml.Automation.Peers;
using UWPYourNote.View;
using static UWPYourNoteLibrary.Util.NotesUtilities;

namespace UWPYourNote.ViewModels
{


    public class HomePageVM : INotifyPropertyChanged
    {


        private long GetMilliSeconds(string time)
        {
            DateTimeOffset milli = DateTime.Parse(time);
            return milli.ToUnixTimeMilliseconds();
        }

        private static HomePageVM homePageVM;
        public static HomePageVM Singleton
        {
            get
            {
                if (homePageVM == null)
                    homePageVM = new HomePageVM();
                return homePageVM;
            }
        }
        public IHomePageView homePageView { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<UWPYourNoteLibrary.Models.Note> SortByModificationtime(ObservableCollection<UWPYourNoteLibrary.Models.Note> notes)
        {
            ObservableCollection<UWPYourNoteLibrary.Models.Note> sortedNotes = new ObservableCollection<UWPYourNoteLibrary.Models.Note>();
            if (notes != null)
            {
                var result = notes.OrderByDescending(a => GetMilliSeconds(a.modifiedDay));
                foreach (UWPYourNoteLibrary.Models.Note note in result)
                {
                    sortedNotes.Add(note);
                }
            }

            return sortedNotes;
        }

        //-------------------------------------------- DISPLAYING NOTES IN GRID VIEW ------------------------------------------------
        public void GetNotes(string userId, bool isSort, TypeOfNote type)
        {
            GetNotesUseCaseRequest request = new GetNotesUseCaseRequest();
            request.UserId = userId;
            request.IsSort = isSort;
            request.Type = type;
            GetNotesUseCase usecase = new GetNotesUseCase(request, new GetNotesPresenterCallBack(this));
            usecase.Action();

        }
        void AssignNotes(ObservableCollection<Note> notes, bool IsSort)
        {
            Page page = (Page)homePageView;
            _ = page?.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                  if (IsSort)
                        notes = SortByModificationtime(notes);
                    homePageVM.NotesDataItemSource = notes;


            });
        }


       
      



        //----------------------------Main Menu List Box---------------------------------------------------

        private bool _personalNotesIsSelected = true;
        public bool PersonalNotesIsSelected
        {
            get { return _personalNotesIsSelected; }
            set
            {
                _personalNotesIsSelected = value;
                OnPropertyChanged();

            }
        }





        private bool _sharedNotesIsSelected = false;
        public bool SharedNotesIsSelected
        {
            get { return _sharedNotesIsSelected; }
            set
            {
                _sharedNotesIsSelected = value;
                OnPropertyChanged();
            }
        }




        private bool _allNotesIsSelected = false;
        public bool AllNotesIsSelected
        {
            get { return _allNotesIsSelected; }
            set
            {
                _allNotesIsSelected = value;
                OnPropertyChanged();
            }
        }

        //----------------------------Note Grid homePageView---------------------------------------------------

   
            
        private ObservableCollection<UWPYourNoteLibrary.Models.Note> _notesDataItemSource = null;
        public ObservableCollection<UWPYourNoteLibrary.Models.Note> NotesDataItemSource
        {
            get { return _notesDataItemSource; }
            set
            {
                _notesDataItemSource = value;
                OnPropertyChanged();
            }
        }

        //------------------------------- CREATING NEW NOTE-----------------------------------------------------
        public void CreateNewNote(Note newNote)
        {
            NotesDataItemSource.Insert(0,newNote);   
            CreateNewNoteUseCaseRequest request = new CreateNewNoteUseCaseRequest();
            request.NewNote = newNote;  
            CreateNewNoteUseCase usecase = new CreateNewNoteUseCase(request, new CreateNewNotePresenterCallBack(this));
            usecase.Execute();
        }

        //---------------------------- UPDATION OF NOTE------------------------------------------
        public void NoteUpdation(Note selectedNote, string title, string content, string modifiedDate, long noteColor)
        {
            int i = NotesDataItemSource.IndexOf(selectedNote);
            Note note = NotesDataItemSource[i];
            NotesDataItemSource.RemoveAt(i);
            note.content = content;
            note.title = title;
            note.modifiedDay = modifiedDate;
            note.noteColor = noteColor;
            NotesDataItemSource.Insert(0, note);
        }

        //-------------------------------DELETION OF NOTE--------------------------------------------------
        public void NoteDeletion(Note selectedNote)
        {
            int i = NotesDataItemSource.IndexOf(selectedNote);
            if (i != -1)
                NotesDataItemSource.RemoveAt(i);
        }


        // PRESENTER CALLBACK 

        private class GetNotesPresenterCallBack : ICallback<GetNotesUseCaseResponse>
        {
            private HomePageVM Presenter;
            public GetNotesPresenterCallBack(HomePageVM presenter)
            {
                Presenter = presenter;
            }

            public void onFailure(GetNotesUseCaseResponse result)
            {
               
                    Presenter?.AssignNotes(result.List, result.IsSort);

            }

            public void onSuccess(GetNotesUseCaseResponse result)
            {
              
                    Presenter?.AssignNotes(result.List, result.IsSort);
            }
        }
      
        private class CreateNewNotePresenterCallBack : ICallback<CreateNewNoteUseCaseResponse>
        {
            private HomePageVM Presenter;
            public CreateNewNotePresenterCallBack(HomePageVM presenter)
            {
                Presenter = presenter;
            }

            public void onFailure(CreateNewNoteUseCaseResponse result)
            {
            }

            public void onSuccess(CreateNewNoteUseCaseResponse result)
            {
                Page page = (Page)Presenter?.homePageView;
                _ = page?.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                {
                    homePageVM.NotesDataItemSource[0].noteId = result.NoteId;
                });
            }
        }


    }
}
