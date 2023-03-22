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
using UWPYourNote.ViewModels;
using UWPYourNoteLibrary.Domain.Contract;
using System.ServiceModel.Dispatcher;
using static UWPYourNote.ViewModels.SignUpPageVM;
using UWPYourNote.ViewModels.Contract;

namespace UWPYourNote.ViewModels
{
    internal class SignUpPageVM
    {
        
        public SignUpPageVM()
        { 
        }

        private static SignUpPageVM _signUpPageViewModel = null;
        public static SignUpPageVM Singleton
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
        public ICheckExistingUser check { get; internal set; }
       

        public void InsertNewUser(string name, string email, string password)
        {
            CreateAccountUseCaseRequest uCAccountCreationRequest = new CreateAccountUseCaseRequest(name, email, password);
            CreateAccountUseCase uCAccountCreation = new CreateAccountUseCase(uCAccountCreationRequest, new InsertNewUserCallBack(this));
            uCAccountCreation.Execute();
        }

        public void IsExistingEmail(string email)
        {
            UWPYourNote.ViewModels.Util.UserUtilities.CheckIfUsersExists(email, new IsExistingEmailCallBack(this));
            // return DBFetch.CheckValidEmail(UserUtilities.userTableName, email);

        }




        public sealed class InsertNewUserCallBack : ICallback<CreateAccountUseCaseResponse>
        {

            private SignUpPageVM SignUpPageVM { get; set; }
            public InsertNewUserCallBack(SignUpPageVM signUpPageVM)
            {
                SignUpPageVM = signUpPageVM;
            }
            public void onSuccess(CreateAccountUseCaseResponse result)
            {
                SignUpPageVM?.View?.NavigateTo();
            }

            public void onFailure(CreateAccountUseCaseResponse result)
            {
                
            }
        }


        public sealed class IsExistingEmailCallBack : ICallback<CheckIfUserExistsUseCaseResponse>
        {
            string value = null;
            private SignUpPageVM SignUpPageVM { get; set; }
            public IsExistingEmailCallBack(SignUpPageVM signUpPageVM)
            {
                SignUpPageVM = signUpPageVM;
            }
            public void onSuccess(CheckIfUserExistsUseCaseResponse result)
            {
                value = "An account already exists for this email address"; 
                SignUpPageVM?.check?.CheckExistingUser(value);
            }

            public void onFailure(CheckIfUserExistsUseCaseResponse result)
            {
                value = null;
                SignUpPageVM?.check?.CheckExistingUser(value);

            }
        }
    }


}


