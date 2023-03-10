using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using YourNoteUWP.Models;
using YourNoteUWP.View;

namespace YourNoteUWP.ViewModels
{
    internal class SignUpPageViewModel {

        private static SignUpPageViewModel _signUpPageViewModel = null;
        public static SignUpPageViewModel SignUpPVM
        {
            get
            {
                if (_signUpPageViewModel == null)
                {
                    _signUpPageViewModel = new SignUpPageViewModel();
                }
                return _signUpPageViewModel;
            }
        }
        public bool IsExistingEmail(string email)
        {
            return DBFetch.CheckValidEmail(DBCreation.userTableName, email);
        }

        public void InsertNewUser(string name, string email, string password)
        {
            Models.User newUser = new Models.User(name, email, password);
            DBUpdation.InsertNewUser(newUser);

        }


    }


}


