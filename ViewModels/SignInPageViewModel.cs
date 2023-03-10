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
using YourNoteUWP.View;
using System.ComponentModel.DataAnnotations;
using Windows.UI.Xaml.Media.Animation;
using YourNoteUWP.Models;
using Windows.System;

namespace YourNoteUWP.ViewModels
{
    internal class SignInPageViewModel 
    {

        private static SignInPageViewModel _signInPageViewModel = null;
        public static SignInPageViewModel SignInPVM
        {
            get
            {
                if (_signInPageViewModel == null)
                {
                    _signInPageViewModel = new SignInPageViewModel();
                }
                return _signInPageViewModel;
            }
        }
        public bool IsExistingEmail(string email)
        {
            return DBFetch.CheckValidEmail(DBCreation.userTableName, email);

        }


    }
  }