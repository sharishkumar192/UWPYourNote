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
using UWPYourNoteLibrary.Domain.Contract;
using Windows.System;
using UWPYourNote.ViewModels;
using UWPYourNoteLibrary.Domain.UseCase;
using UWPYourNote.ViewModels.Contract;
namespace UWPYourNote.ViewModels
{
    internal class SignInPageVM : INotifyPropertyChanged
    {
        private static SignInPageVM _signInPageViewModel = null;
        public static SignInPageVM Singleton
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

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //----------------------------Column Divider---------------------------------------------------

        private Visibility _columnDividerVisibility = Visibility.Collapsed;

        public Visibility ColumnDividerVisibility
        {
            get { return _columnDividerVisibility; }
            set
            {
                _columnDividerVisibility = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<UWPYourNoteLibrary.Models.User> _frequentEmailItemSource;
        public ObservableCollection<UWPYourNoteLibrary.Models.User> FrequentEmailItemSource
        {
            get { return _frequentEmailItemSource; }
            set
            {
                _frequentEmailItemSource = value;

                OnPropertyChanged();
                IsChangeInRecentLogInUsersList();
            }

        }

        public void IsChangeInRecentLogInUsersList()
        {
            if (FrequentEmailItemSource == null || FrequentEmailItemSource.Count == 0)
            {
                FrequentEmailBoxVisibility = Visibility.Collapsed;
            }
            else
            {
                FrequentEmailBoxVisibility = Visibility.Visible;
            }
        }
        public void GetRecentLogInUsers()
        {
            RecentLogInUseCaseRequest request = new RecentLogInUseCaseRequest();
            RecentLogInUseCase usecase = new RecentLogInUseCase(request, new RecentLogInUseCasePresenterCallBack(this));
            usecase.Execute();

        }

        private Visibility _frequentEmailBoxVisibility = Visibility.Collapsed;

        public Visibility FrequentEmailBoxVisibility
        {
            get { return _frequentEmailBoxVisibility; }
            set
            {
                _frequentEmailBoxVisibility = value;
                ShowColumnDivider();
                OnPropertyChanged();

            }
        }

        public void ShowColumnDivider()
        {
            if (FrequentEmailBoxVisibility == Visibility.Collapsed)
            {
                ColumnDividerVisibility = Visibility.Collapsed;
            }
            else
            {
                ColumnDividerVisibility = Visibility.Visible;
            }
        }


        public ICheckExistingUser check { get; internal set; }
        public ISignInPageView signInPageView { get; internal set; }    
        public void IsExistingUser(string email)
        {
            UWPYourNote.ViewModels.Util.UserUtilities.CheckIfUsersExists(email, new IsExistingUserPresenterCallBack(this));
            // return DBFetch.CheckValidEmail(DBCreation.userTableName, email);

        }

        public void ValidateLogInUser(string userId, string password)
        {

            ValidateCredentialsUseCaseRequest request = new ValidateCredentialsUseCaseRequest(userId, password);
            ValidateCredentialsUseCase usecase = new ValidateCredentialsUseCase(request, new ValidateCredentialsPresenterCallBack(this));
            usecase.Execute();
          // DBFetch.ValidateUser(DBCreation.userTableName, userId, password);
        }


        private class  IsExistingUserPresenterCallBack : ICallback<CheckIfUserExistsUseCaseResponse>
        {
            SignInPageVM _signInPageVM;
            string value = null;
            public IsExistingUserPresenterCallBack(SignInPageVM signInPageVM)
            {
                _signInPageVM = signInPageVM;
            }
           

            public void onSuccess(CheckIfUserExistsUseCaseResponse result)
            {
                value = null;
                _signInPageVM?.check?.CheckExistingUser(value);
            }

            public void onFailure(CheckIfUserExistsUseCaseResponse result)
            {
                value = "Incorrect email address";
                _signInPageVM?.check?.CheckExistingUser(value);

            }
        }

        private class ValidateCredentialsPresenterCallBack : ICallback<ValidateCredentialsUseCaseResponse>
        {
            private SignInPageVM _signInPageVM;
            public ValidateCredentialsPresenterCallBack(SignInPageVM signInPageVM)
            {
                _signInPageVM = signInPageVM;
            }


            public void onSuccess(ValidateCredentialsUseCaseResponse result)
            {
              
                _signInPageVM?.signInPageView?.NavigateToOnSuccess(result.user);
            }

            public void onFailure(ValidateCredentialsUseCaseResponse result)
            {
             
                _signInPageVM?.signInPageView?.NavigateToOnFailure();

            }

        }

        private class RecentLogInUseCasePresenterCallBack : ICallback<RecentLogInUseCaseResponse>
        {
            private SignInPageVM _signInPageVM;
            public RecentLogInUseCasePresenterCallBack(SignInPageVM signInPageVM)
            {
                _signInPageVM = signInPageVM;
            }


            public void onSuccess(RecentLogInUseCaseResponse result)
            {
                Page page = (Page)(_signInPageVM?.signInPageView);

                _ = page?.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                {
                    if (_signInPageVM != null)
                        _signInPageVM.FrequentEmailItemSource = result.List;

                });

  

            }

            public void onFailure(RecentLogInUseCaseResponse result)
            {
                Page page = (Page)(_signInPageVM?.signInPageView);

                _ = page?.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                {
                    if (_signInPageVM != null)
                        _signInPageVM.FrequentEmailItemSource = result.List;

                });

              
            }

        }

    }


  }