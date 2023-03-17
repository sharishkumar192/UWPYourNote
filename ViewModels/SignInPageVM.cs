using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using UWPYourNote.View;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Xaml.Media.Animation;
using UWPYourNoteLibrary.Models;
using UWPYourNoteLibrary.Domain;
using Windows.System;

namespace UWPYourNote.ViewModels
{
    internal class SignInPageVM 
    {

        private static SignInPageVM _signInPageViewModel = null;
        public static SignInPageVM SignInPVM
        {
            get
            {
                if (_signInPageViewModel == null)
                {
                    _signInPageViewModel = new SignInPageVM();
                }
                return _signInPageViewModel;
            }
        }
        public bool IsExistingEmail(string email)
        {
            return DBFetch.CheckValidEmail(DBCreation.userTableName, email);

        }

        public  Tuple<UWPYourNoteLibrary.Models.User, bool> ValidateLogInUser(string userId, string password)
        {
        // UCBase uCAccountCreation = new UCAccountCreation();
         //   uCAccountCreation.Execute(uCAccountCreation);

            return DBFetch.ValidateUser(DBCreation.userTableName, userId, password);
        }

        private class PresenterCallBack
        {
            void OnSuccess()
            {

            }

            void OnFailure() 
            { 

            }
        }


    }

   
  }