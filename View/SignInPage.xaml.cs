using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWPYourNote.ViewModels;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Animation;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UWPYourNoteLibrary.Util;
using UWPYourNoteLibrary.Models;
using UWPYourNote.ViewModels.Contract;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPYourNote.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignInPage : Page, ISignInPageView, ICheckExistingUser, INotifyPropertyChanged
    {
        private Frame _frame;
        SignInPageVM signInPageVM;
        public SignInPage()
        {
            this.InitializeComponent();

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {   _frame = e.Parameter as Frame;
                signInPageVM = new SignInPageVM();
            signInPageVM.signInPageView = this; 
            signInPageVM.GetRecentLogInUsers();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        //------------------------------------------Email TextBox---------------------------------------------------

        public void CheckExistingUser(string result)
        {
            //       value = "An account already exists for this email address";
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                if (result == null)
                {
                    EmailToolTipContent = "";
                    EmailCheckVisibility = Visibility.Collapsed;
                }
                else
                {
                    EmailToolTipContent = result;
                    EmailCheckVisibility = Visibility.Visible;

                }
            });
       
        }
        public  void CheckAlreadyExistingEmail(string email)
        {

            signInPageVM.check = this;
            signInPageVM.IsExistingUser(email);
       
     
        }

        public string IsEmailCheck(string email)
        {
            string checkNullOrEmpty = UserUtilities.CheckNullOrEmpty(email);

            if (checkNullOrEmpty != null)
                return checkNullOrEmpty;

            string checkValid = UserUtilities.CheckValidEmail(email);

            if (checkValid != null)
                return checkValid;

            CheckAlreadyExistingEmail(email);
            return null;
        }



        public string CheckNullOrEmpty(string email)
        {
            return CheckNullOrEmpty(email);

        }


        //Text Box

        private string _emailBoxContent = "";
        public string EmailBoxContent
        {
            get { return _emailBoxContent; }
            set
            {
                _emailBoxContent = value;
                EmailBoxLostFocus();
                OnPropertyChanged();
            }
        }
        public string EmailBoxPlaceHolderText
        {
            get { return "Email address"; }
        }


        // Font Icon
        private Visibility _emailCheckVisibility = Visibility.Collapsed;

        public Visibility EmailCheckVisibility
        {
            get { return _emailCheckVisibility; }
            set
            {
                _emailCheckVisibility = value;
                ShowEmailToolTip();
                OnPropertyChanged();
            }
        }
        //Tool Tip
        private string _emailToolTipContent = "";

        public string EmailToolTipContent
        {
            get { return _emailToolTipContent; }
            set
            {
                _emailToolTipContent = value;
                OnPropertyChanged();
            }
        }


        private Visibility _emailToolTipVisibility = Visibility.Collapsed;

        public Visibility EmailToolTipVisibility
        {
            get { return _emailToolTipVisibility; }
            set
            {
                _emailToolTipVisibility = value;
                OnPropertyChanged();
            }
        }

        public void EmailBoxLostFocus()
        {
            string value = IsEmailCheck(EmailBoxContent);
            if (value == null)
            {
                EmailToolTipContent = "";
                EmailCheckVisibility = Visibility.Collapsed;
            }
            else
            {
                EmailToolTipContent = value;
                EmailCheckVisibility = Visibility.Visible;

            }

        }

        public void ShowEmailToolTip()
        {
            if (EmailCheckVisibility == Visibility.Collapsed)
            {
                EmailToolTipVisibility = Visibility.Collapsed;
            }
            else
            {
                EmailToolTipVisibility = Visibility.Visible;
            }
        }

        public void EmailBoxTextChanged()
        {
            EmailToolTipContent = "";
            EmailCheckVisibility = Visibility.Collapsed;
        }


        //-------------------------------------------------- Password Box --------------------------------------------------------


        //Password Box
        private string _passwordBoxPassword = "";
        public string PasswordBoxPassword
        {
            get { return _passwordBoxPassword; }
            set
            {
                _passwordBoxPassword = value;
                PasswordBoxLostFocus();
                OnPropertyChanged();
            }
        }


        private PasswordRevealMode _passwordBoxRevealMode = PasswordRevealMode.Hidden;
        public PasswordRevealMode PasswordBoxRevealMode
        {
            get { return _passwordBoxRevealMode; }
            set
            {
                _passwordBoxRevealMode = value;
                OnPropertyChanged();
            }
        }

        public string PasswordBoxPlaceHolderText
        {
            get { return "Password"; }
        }

        //Check Box
        private bool _revealModeCheckBoxIsChecked = false;
        public bool RevealModeCheckBoxIsChecked
        {
            get { return _revealModeCheckBoxIsChecked; }
            set
            {
                _revealModeCheckBoxIsChecked = value;
                OnPropertyChanged();
            }
        }


        // Font Icon
        private Visibility _passwordCheckVisibility = Visibility.Collapsed;
        public Visibility PasswordCheckVisibility
        {
            get { return _passwordCheckVisibility; }
            set
            {
                _passwordCheckVisibility = value;
                ShowPasswordToolTip();
                OnPropertyChanged();
            }
        }


        //Tool Tip
        private string _passwordToolTipContent = "";

        public string PasswordToolTipContent
        {
            get { return _passwordToolTipContent; }
            set
            {
                _passwordToolTipContent = value;
                OnPropertyChanged();
            }
        }

        private Visibility _passwordToolTipVisibility = Visibility.Collapsed;
        public Visibility PasswordToolTipVisibility
        {
            get { return _passwordToolTipVisibility; }
            set
            {
                _passwordToolTipVisibility = value;
                OnPropertyChanged();
            }
        }

        public void RevealModeCheckBoxChecked()
        {
            if (RevealModeCheckBoxIsChecked == false)
            {
                RevealModeCheckBoxIsChecked = true;
                PasswordBoxRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                RevealModeCheckBoxIsChecked = false;
                PasswordBoxRevealMode = PasswordRevealMode.Hidden;
            }
        }

        public void PasswordBoxLostFocus()
        {
            string value = UserUtilities.IsPasswordCheck(PasswordBoxPassword);
            if (value == null)
            {
                PasswordToolTipContent = "";
                PasswordCheckVisibility = Visibility.Collapsed;
            }
            else
            {
                PasswordToolTipContent = value;
                PasswordCheckVisibility = Visibility.Visible;


            }
        }

        public void PasswordBoxTextChanged()
        {
            PasswordToolTipContent = "";
            PasswordCheckVisibility = Visibility.Collapsed;
        }


        public void ShowPasswordToolTip()
        {
            if (PasswordCheckVisibility == Visibility.Collapsed)
            {
                PasswordToolTipVisibility = Visibility.Collapsed;
            }
            else
            {
                PasswordToolTipVisibility = Visibility.Visible;
            }
        }

        //---------------------------------------------- Frequent Users ListView ----------------------------------------------------

        
        


        public void FrequentEmailItemClick(object sender, ItemClickEventArgs e)
        {
            var frequentUser = (UWPYourNoteLibrary.Models.User)e.ClickedItem;

            EmailBoxContent = frequentUser.userId;
        }


 

      


       
        //------------------------------------------- Navigation Buttons-----------------------------------------
        public void LogInButtonClick()
        {
            var x = this.RequestedTheme;
            EmailBoxLostFocus();
            PasswordBoxLostFocus();
            if (EmailCheckVisibility == Visibility.Collapsed &&
                PasswordCheckVisibility == Visibility.Collapsed)
            {
             
                signInPageVM.signInPageView = this;
                signInPageVM.ValidateLogInUser(EmailBoxContent, PasswordBoxPassword);


            }

        }

        public void NavigateToSignUpClick()
        {
            //   _frame.Navigate(typeof(SignUpPage), _frame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
            NavigateToSignUpPage();
        }

        public void NavigateToSignUpPage()
        {
            _frame.Navigate(typeof(SignUpPage), _frame, null);

//            _frame.Navigate(typeof(SignUpPage), _frame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        public void NavigateToHomePage(User loggedUser)
        {
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                Tuple<Frame, UWPYourNoteLibrary.Models.User> tuple = new Tuple<Frame, UWPYourNoteLibrary.Models.User>(_frame, loggedUser);
                _frame.Navigate(typeof(HomePage), tuple);
            });
        }
        public void NavigateToSignInPage()
        {
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
            if (EmailToolTipContent.Equals("") == true)
            {
                
                     EmailToolTipContent = "";
                EmailCheckVisibility = Visibility.Collapsed;
                
            }
            PasswordToolTipContent = "Incorrect Email address or password";
            PasswordCheckVisibility = Visibility.Visible;
            });
        }

        public void NavigateToOnFailure()
        {
            NavigateToSignInPage();
        }
        public void NavigateToOnSuccess(User loggedUser)
        {
            NavigateToHomePage(loggedUser);
        }

      
    }
}