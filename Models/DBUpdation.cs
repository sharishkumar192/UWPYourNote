using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Graphics.Printing.PrintTicket;
using Windows.System;
using YourNoteUWP;
using YourNoteUWP.Models;

namespace YourNoteUWP
{
    public class DBUpdation
    {

        // ----------------------------------------SIGN UP PAGE DB UPDATION----------------------------------------

        //Creates new currentUser 
        public static void InsertNewUser(Models.User user) //Needed
        {
            SQLiteConnection conn = DBCreation.OpenConnection();
            try
            {


                   string query = $"INSERT INTO {DBCreation.userTableName}(NAME, USERID, PASSWORD) VALUES ( @name, @userId, @password);";


                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteParameter[] parameters = new SQLiteParameter[3];
                //parameters[0] = new SQLiteParameter("@userTableName", DBCreation.userTableName);
                parameters[0] = new SQLiteParameter("@name", user.name);
                parameters[1] = new SQLiteParameter("@userId", user.userId);
                parameters[2] = new SQLiteParameter("@password", user.password);
                command.Parameters.Add(parameters[0]);
                command.Parameters.Add(parameters[1]);
                command.Parameters.Add(parameters[2]);
                // command.Parameters.Add(parameters[3]);
                command.ExecuteNonQuery();
                conn.Close();





            }
           catch(Exception e) { Logger.WriteLog(e.Message);  }
            finally
            {
                conn.Close();

            }

            //sqlite_cmd.CommandText = $"INSERT INTO {DBCreation.userTableName}(UserId, Password,Name) VALUES ('{currentUser.Userid}' , ' " + { currentUser.Password} + "','" + currentUser.Name + "');";

        }


        // ----------------------------------------ACCOUNT PAGE DB UPDATION----------------------------------------

        //Creates new note 
        public static long InsertNewNote(string tableName, Note newNote)// Needed 
        {

            SQLiteConnection conn = DBCreation.OpenConnection();
            long noteId = 0;
            string query1 = $"INSERT INTO {tableName}  (USERID, TITLE, CONTENT, NOTECOLOR, CREATIONDAY, MODIFIEDDAY) VALUES (@userId, @title, @content, @noteColor, @creationDay, @modifiedDay);";
            string query2 = $"SELECT seq FROM sqlite_sequence where name =  '{tableName}' ; ";
            try
            {


                SQLiteCommand command = new SQLiteCommand(query1, conn);
                SQLiteParameter[] parameters = new SQLiteParameter[6];
                parameters[0] = new SQLiteParameter("@userId", newNote.userId);
                parameters[1] = new SQLiteParameter("@title", newNote.title);
                parameters[2] = new SQLiteParameter("@content", newNote.content);
                parameters[3] = new SQLiteParameter("@noteColor", newNote.noteColor);
                parameters[4] = new SQLiteParameter("@creationDay", newNote.creationDay);
                parameters[5] = new SQLiteParameter("@modifiedDay", newNote.modifiedDay);
                command.Parameters.Add(parameters[0]);
                command.Parameters.Add(parameters[1]);
                command.Parameters.Add(parameters[2]);
                command.Parameters.Add(parameters[3]);
                command.Parameters.Add(parameters[4]);
                command.Parameters.Add(parameters[5]);

                command.ExecuteNonQuery();

                command.Parameters.Remove(parameters[0]);
                command.Parameters.Remove(parameters[1]);
                command.Parameters.Remove(parameters[2]);
                command.Parameters.Remove(parameters[3]);
                command.Parameters.Remove(parameters[4]);
                command.Parameters.Remove(parameters[5]);
              //  string query3 = "SELECT seq FROM  sqlite_sequence where name = 'NotesTable' ";
                command.CommandText = query2;

                noteId = (long)command.ExecuteScalar();


                conn.Close();



            }
           catch(Exception e) { Logger.WriteLog(e.Message);  }
            finally
            {
                conn.Close();

            }
            return noteId;

        }


        // ----------------------------------------NOTE DISPLAY PAGE DB UPDATION----------------------------------------

        //Creates a new entry for the shared note
        public static void InsertSharedNote(string sharedTableName, string sharedUserId, long noteId)// Needed
        {
            string query = $"INSERT INTO {sharedTableName} VALUES (@SHAREDUSERID, @NOTEID);";
            SQLiteConnection conn = DBCreation.OpenConnection();
            try
            {


                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteParameter[] parameters = new SQLiteParameter[2];
                parameters[0] = new SQLiteParameter("@sharedUserId", sharedUserId);
                parameters[1] = new SQLiteParameter("@noteId", noteId);
                command.Parameters.Add(parameters[0]);
                command.Parameters.Add(parameters[1]);
                command.ExecuteNonQuery();
                conn.Close();






            }
           catch(Exception e) { Logger.WriteLog(e.Message);  }
            finally
            {
                conn.Close();

            }
        }
        //Updation of the Note
        public static void UpdateNote(string notesTableName, string title, string content, long noteId, string modifiedDay)// Needed
        {
            string query = $"UPDATE {notesTableName} SET TITLE = @title, CONTENT= @content, MODIFIEDDAY = @modifiedDay WHERE NOTEID = @noteId  ;";
            SQLiteConnection conn = DBCreation.OpenConnection();
            try
            {

                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteParameter[] parameters = new SQLiteParameter[4];
                parameters[0] = new SQLiteParameter("@title", title);
                parameters[1] = new SQLiteParameter("@content", content);
                parameters[2] = new SQLiteParameter("@modifiedDay", modifiedDay);
                parameters[3] = new SQLiteParameter("@noteId", noteId);


                command.Parameters.Add(parameters[0]);
                command.Parameters.Add(parameters[1]);
                command.Parameters.Add(parameters[2]);
                command.Parameters.Add(parameters[3]);
                command.ExecuteNonQuery();
                conn.Close();




            }
           catch(Exception e) { Logger.WriteLog(e.Message);  }
            finally
            {
                conn.Close();

            }

        }
        //Updation of the Note Title
        public static void UpdateNoteTitle(string notesTableName, string title, long noteId, string modifiedDay)// Needed
        {
            string query = $"UPDATE {notesTableName} SET TITLE= @title, MODIFIEDDAY = @modifiedDay WHERE NOTEID = @noteId  ;";
            SQLiteConnection conn = DBCreation.OpenConnection();
            try
            {

                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteParameter[] parameters = new SQLiteParameter[3];
                parameters[0] = new SQLiteParameter("@title", title);
                parameters[1] = new SQLiteParameter("@modifiedDay", modifiedDay);
                parameters[2] = new SQLiteParameter("@noteId", noteId); 


                command.Parameters.Add(parameters[0]);
                command.Parameters.Add(parameters[1]);
                command.Parameters.Add(parameters[2]);
                command.ExecuteNonQuery();
                conn.Close();




            }
           catch(Exception e) { Logger.WriteLog(e.Message);  }
            finally
            {
                conn.Close();

            }

        }

        public static void UpdateNoteCount(string notesTableName, long searchCount, long noteId)
        {
            string query = $"UPDATE {notesTableName} SET  SEARCHCOUNT = @count  WHERE NOTEID = @noteId  ;";
            SQLiteConnection conn = DBCreation.OpenConnection();
            try
            {

                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteParameter[] parameters = new SQLiteParameter[2];
                parameters[0] = new SQLiteParameter("@count", searchCount);
                parameters[1] = new SQLiteParameter("@noteId", noteId);


                command.Parameters.Add(parameters[0]);
                command.Parameters.Add(parameters[1]);
                command.ExecuteNonQuery();
                conn.Close();
            }
           catch(Exception e) { Logger.WriteLog(e.Message);  }
            finally
            {
                conn.Close();

            }


        }

        //Updation of the Note Content
        public static void UpdateNoteContent(string notesTableName, string content, long noteId, string modifiedDay)// Needed
        {
            string query = $"UPDATE {notesTableName} SET  CONTENT= @content, MODIFIEDDAY = @modifiedDay WHERE NOTEID = @noteId  ;";
            SQLiteConnection conn = DBCreation.OpenConnection();
            try
            {

                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteParameter[] parameters = new SQLiteParameter[3];
                parameters[0] = new SQLiteParameter("@content", content);
                parameters[1] = new SQLiteParameter("@modifiedDay", modifiedDay);
                parameters[2] = new SQLiteParameter("@noteId", noteId);


                command.Parameters.Add(parameters[0]);
                command.Parameters.Add(parameters[1]);
                command.Parameters.Add(parameters[2]);
                command.ExecuteNonQuery();
                conn.Close();
            }
           catch(Exception e) { Logger.WriteLog(e.Message);  }
            finally
            {
                conn.Close();

            }

        }

        //Delete the Note
        public static void DeleteNote(string notesTableName, string sharedTableName, long noteId)// Needed 
        {
            SQLiteCommandBuilder sqlCommandBuilder = new SQLiteCommandBuilder();


            string query = $"DELETE FROM {notesTableName} WHERE NOTEID  = @noteId ; ";
         
            SQLiteConnection conn = DBCreation.OpenConnection();

            try
            {


                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteParameter parameters = new SQLiteParameter("@noteId", noteId);
                command.Parameters.Add(parameters);
                command.ExecuteNonQuery();                
                conn.Close();


            }
           catch(Exception e) { Logger.WriteLog(e.Message);  }
            finally
            {
                conn.Close();

            }

        }

        public static void UpdateNoteColor(string tableName, long noteId, long noteColor, string modifiedDay)
        {
            string query = $"UPDATE  {tableName} SET NOTECOLOR = @noteColor, MODIFIEDDAY = @modifiedDay  WHERE NOTEID = @noteId ; ";
            SQLiteConnection conn = DBCreation.OpenConnection();
            try
            {
                SQLiteCommand command = new SQLiteCommand(query, conn);
                SQLiteParameter[] parameters = new SQLiteParameter[3];
                parameters[0] = new SQLiteParameter("@noteColor", noteColor);
                parameters[1] = new SQLiteParameter("@modifiedDay", modifiedDay);
                parameters[2] = new SQLiteParameter("@noteId", noteId);

                command.Parameters.Add(parameters[0]);
                command.Parameters.Add(parameters[1]);
                command.Parameters.Add(parameters[2]);
                command.ExecuteNonQuery();
                conn.Close();

            }
            catch (Exception e) { Logger.WriteLog(e.Message); }
            finally
            {
                conn.Close();

            }


        }






    }
}
