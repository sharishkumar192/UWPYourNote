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
using UWPYourNoteLibrary.Models;
using UWPYourNoteLibrary.Domain;
using UWPYourNoteLibrary;
using UWPYourNote.View;
using System.Diagnostics;
using YourNoteUWP.ViewModels;
using System.ServiceModel.Dispatcher;
namespace UWPYourNote.ViewModels
{
    internal class SignUpPageVM
    {
        private IView _View;

        public SignUpPageVM()
        { 
        }

        private static SignUpPageVM _signUpPageViewModel = null;
        public static SignUpPageVM SignUpPVM
        {
            get
            {
                if (_signUpPageViewModel == null)
                {
                    _signUpPageViewModel = new SignUpPageVM();
                }
                return _signUpPageViewModel;
            }
        }

        public IView View { get; internal set; }

        public bool IsExistingEmail(string email)
        {
            return DBFetch.CheckValidEmail(DBCreation.userTableName, email);
        }

        public void InsertNewUser(string name, string email, string password)
        {
      
            UCAccountCreationRequest uCAccountCreationRequest = new UCAccountCreationRequest(name, email, password);
            UCAccountCreation uCAccountCreation = new UCAccountCreation(uCAccountCreationRequest, new SignUpPageVMPresenterCallBack(this));
            uCAccountCreation.Execute();
            //DBUpdation.InsertNewUser(newUser);

        }

        public sealed class SignUpPageVMPresenterCallBack : IPresenterCallback
        {

            private SignUpPageVM SignUpPageVM { get; set; }
            public SignUpPageVMPresenterCallBack(SignUpPageVM signUpPageVM)
            {
                SignUpPageVM = signUpPageVM;
            }
            public void onFailure()
            {
               
            }

            public void onSuccess()
            {
                SignUpPageVM?.View?.NavigateTo();
            }
        }
    }


}


