using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace YourNoteUWP.Models
{
    public class Note
    {
        public string userId;
        public long noteId;
        public string title;
        public string content;
        public long noteColor;
        public long searchCount;
        public string creationDay;
        public string modifiedDay;
        public Note(string userId, long noteId, string title, string content, long noteColor, long searchCount, string creationDay, string modifiedDay)
        {
            this.userId = userId;
            this.noteId = noteId;
            this.title = title;
            this.content = content;
            this.noteColor = noteColor;
            this.searchCount = searchCount;
        }

        public Note(Note copyNote)
        {
            this.userId = copyNote.userId;
            this.noteId = copyNote.noteId;
            this.title = copyNote.title;
            this.content = copyNote.content;
            this.noteColor = copyNote.noteColor;
            this.searchCount = copyNote.searchCount;
        }
        public Note(string userId, string title, string content, long noteColor)
        {
            this.userId = userId;
            this.title = title;
            this.content = content;
            this.noteColor = noteColor;
        }

        public Note(string userId, string title, string content, long noteColor, string creationDay, string modifiedDay) 
        {

            this.userId = userId;
            this.title = title;
            this.content = content;
            this.noteColor = noteColor;
            this.creationDay = creationDay;
            this.modifiedDay = modifiedDay;
        }



     public Note(long noteId, string title, string content, long noteColor, long searchCount, string modifiedDay)
        {
            this.noteId = noteId;
            this.title = title;
            this.content = content;
            this.noteColor = noteColor;
            this.searchCount = searchCount;
            this.modifiedDay = modifiedDay;
        }

        public Note(string userId, long noteId, string title, string content, long noteColor, string modifiedDay)
        {
            this.userId = userId;
            this.noteId = noteId;
            this.title = title;
            this.content = content;
            this.noteColor = noteColor;
            this.modifiedDay = modifiedDay;
        }
 


    }





}
