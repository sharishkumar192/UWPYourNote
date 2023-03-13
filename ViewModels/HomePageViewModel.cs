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
using UWPYourNoteLibrary.Models;
namespace UWPYourNote.ViewModels
{
    internal class HomePageViewModel
    {
       
     
        private  long GetMilliSeconds(string time)
        {
            DateTimeOffset milli = DateTime.Parse(time);
            return milli.ToUnixTimeMilliseconds();
        }
        

        private  ObservableCollection<UWPYourNoteLibrary.Models.Note> SortByModificationtime(ObservableCollection<UWPYourNoteLibrary.Models.Note> notes)
        {
            ObservableCollection<UWPYourNoteLibrary.Models.Note> sortedNotes = new ObservableCollection<UWPYourNoteLibrary.Models.Note>();
            if (notes != null)
            {
                var result = notes.OrderByDescending(a => GetMilliSeconds(a.modifiedDay));
                foreach(UWPYourNoteLibrary.Models.Note note in result)
                {
                    sortedNotes.Add(note);
                }
            }

            return sortedNotes; 
        }

        public static ObservableCollection<UWPYourNoteLibrary.Models.Note> GetPersonalNotes(string userId, bool isSort)
        {
            HomePageViewModel apvm = new HomePageViewModel();
            var notes = DBFetch.GetPersonalNotes(DBCreation.notesTableName, userId);
            if (isSort == true)
                return apvm.SortByModificationtime(notes);
            return notes;
        }

        public static ObservableCollection<UWPYourNoteLibrary.Models.Note> GetSharedNotes(string userId, bool isSort)
        {
            HomePageViewModel apvm = new HomePageViewModel();
            var  notes = DBFetch.GetSharedNotes(DBCreation.notesTableName, DBCreation.sharedTableName, userId);
            if (isSort == true)
               return apvm.SortByModificationtime(notes);
            return notes;
        }



        public static ObservableCollection<UWPYourNoteLibrary.Models.Note> GetAllNotes(string userId, bool isSort)
        {
            HomePageViewModel apvm = new HomePageViewModel();
            var allNotes = new ObservableCollection<UWPYourNoteLibrary.Models.Note>();
        
                var pnotes = GetPersonalNotes(userId, false);
                var snotes = GetSharedNotes(userId, false);
        
            if (pnotes != null)
                foreach (Note notes in pnotes)
                {
                    allNotes.Add(notes);
                }
            if (snotes!= null)
                foreach (Note notes in snotes)
                {
                    allNotes.Add(notes);
                }

            if (isSort == true)
                return apvm.SortByModificationtime(allNotes);
            return allNotes;

        }
        public ObservableCollection<UWPYourNoteLibrary.Models.Note> GetRecentNotes(string userId)
        {
            ObservableCollection<UWPYourNoteLibrary.Models.Note> recentNotes = null;
            ObservableCollection<UWPYourNoteLibrary.Models.Note> personalNotes = GetPersonalNotes(userId, false);
            ObservableCollection<UWPYourNoteLibrary.Models.Note> sharedNotes = GetSharedNotes(userId, false);
            if (personalNotes == null)
                    personalNotes = new ObservableCollection<UWPYourNoteLibrary.Models.Note>();
            if (sharedNotes != null)
            {
                foreach (Note note in sharedNotes)
                    personalNotes.Add(note);
            }
            personalNotes.OrderByDescending(note => note.searchCount);
            foreach (Note note in personalNotes)
            {
                if (note.searchCount > 0)
                {
                    if(recentNotes == null)
                        recentNotes = new ObservableCollection<UWPYourNoteLibrary.Models.Note>();  
                    recentNotes.Add(note);
                }
                    
            }
            return recentNotes;
        }

        public ObservableCollection<UWPYourNoteLibrary.Models.Note> GetSuggestedNote(string userId, string title)
        {
           return SortByModificationtime(DBFetch.GetSuggestedNotes(DBCreation.notesTableName, userId, title));
        }
        public long CreateNewNote(Note newNote)
        {
          return DBUpdation.InsertNewNote(DBCreation.notesTableName, newNote);
        }
    }


}
