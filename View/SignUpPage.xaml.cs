using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using UWPYourNoteLibrary.Util;
using UWPYourNote.ViewModels;
using UWPYourNote.ViewModels.Contract;
using Windows.UI.ViewManagement;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPYourNote.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUpPage : Page, IView, ICheckExistingUser, INotifyPropertyChanged
    {
        private SignUpPageVM signUpPageVM;
        private Frame _frame;
        public SignUpPage()
        {
            this.InitializeComponent();

        }
        public void NavigateToSignInPage()
        {
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                _frame.Navigate(typeof(SignInPage), _frame);
            });
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _frame = e.Parameter as Frame;
            signUpPageVM = SignUpPageVM.Singleton;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //------------------------------------------Name TextBox---------------------------------------------------

        public string IsNameCheck(string check)
        {
            string checkNullOrEmpty = UWPYourNoteLibrary.Util.UserUtilities.CheckNullOrEmpty(check);
            if (checkNullOrEmpty != null)
                return checkNullOrEmpty;
            return null;
        }

        //Text Box

        private string _nameboxContent = "";
        public string NameBoxContent
        {
            get { return _nameboxContent; }
            set
            {
                _nameboxContent = value;

                NameBoxLostFocus();
                OnPropertyChanged();

            }
        }

        private string _nameBoxPlaceHolderText = "Name";

        public string NameBoxPlaceHolderText
        {
            get { return _nameBoxPlaceHolderText; }

        }


        //Font Icon
        private Visibility _nameCheckVisibility = Visibility.Collapsed;
        public Visibility NameCheckVisibility
        {
            get { return _nameCheckVisibility; }
            set
            {

                _nameCheckVisibility = value;
                ShowNameToolTip();
                OnPropertyChanged();
            }
        }


        // Tool Tip

        private string _nameToolTipContent = "";
        public string NameToolTipContent
        {
            get { return _nameToolTipContent; }
            set
            {
                _nameToolTipContent = value;
                OnPropertyChanged();
            }
        }


        private Visibility _nameToolTipVisibility = Visibility.Collapsed;
        public Visibility NameToolTipVisibility
        {
            get { return _nameToolTipVisibility; }
            set
            {
                _nameToolTipVisibility = value;
                OnPropertyChanged();

            }
        }

        public void NameBoxTextChanged()
        {
            NameToolTipContent = "";
            NameCheckVisibility = NameCheckBorderVisibility = Visibility.Collapsed;
        }

        public void NameBoxLostFocus()
        {

            string value = IsNameCheck(NameBoxContent);
            if (value == null)
            {
                NameToolTipContent = "";
                NameCheckVisibility = NameCheckBorderVisibility = Visibility.Collapsed;

            }
            else
            {
                NameToolTipContent = value;
                NameCheckVisibility = NameCheckBorderVisibility = Visibility.Visible;

            }
        }

        public void ShowNameToolTip()
        {
            if (NameCheckVisibility == Visibility.Collapsed)
            {
                NameToolTipVisibility = NameCheckBorderVisibility = Visibility.Collapsed;
            }
            else
            {
                NameToolTipVisibility = NameCheckBorderVisibility = Visibility.Visible;
            }
        }

        //------------------------------------------Email TextBox---------------------------------------------------

        public void CheckExistingUser(string result)
        {
            //   string value = null;
            //  if(result)
            //        value = "An account already exists for this email address";
            _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                if (result == null)
                {
                    EmailToolTipContent = "";
                    EmailCheckVisibility = EmailCheckBorderVisibility = Visibility.Collapsed;
                }
                else
                {
                    EmailToolTipContent = result;
                    EmailCheckVisibility = EmailCheckBorderVisibility = Visibility.Visible;

                }

            });
           
        }
        public  void CheckAlreadyExistingEmail(string email)
        {
            signUpPageVM.check = this;
            signUpPageVM.IsExistingEmail(email);
       
        }
        private string IsEmailCheck(string email)
        {
            string checkNullOrEmpty = UWPYourNoteLibrary.Util.UserUtilities.CheckNullOrEmpty(email);

            if (checkNullOrEmpty != null)
            {
                return checkNullOrEmpty;
            }

            string checkValid = UWPYourNoteLibrary.Util.UserUtilities.CheckValidEmail(email);

            if(checkNullOrEmpty == null && checkValid==null)
            {
                CheckAlreadyExistingEmail(email);
            }
            if (checkValid != null)
            {
                return checkValid;
            }
            return null;
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
            get { return "Email address";  }
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
                EmailCheckVisibility = EmailCheckBorderVisibility = Visibility.Collapsed;
            }
            else
            {
                EmailToolTipContent = value;
                EmailCheckVisibility = EmailCheckBorderVisibility = Visibility.Visible;

            }

        }

        public void ShowEmailToolTip()
        {
            if (EmailCheckVisibility == Visibility.Collapsed)
            {
                EmailToolTipVisibility = EmailCheckBorderVisibility = Visibility.Collapsed;
            }
            else
            {
                EmailToolTipVisibility = EmailCheckBorderVisibility = Visibility.Visible;
            }
        }

        public void EmailBoxTextChanged()
        {
            EmailToolTipContent = "";
            EmailCheckVisibility = Visibility.Collapsed;
        }

        //------------------------------------------Password Box---------------------------------------------------
       





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

        //Font Icon
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

        // Tool Tip
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


        // Check Box
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


        public void PasswordBoxTextChanged()
        {
            PasswordToolTipContent = "";
            PasswordCheckVisibility = PasswordCheckBorderVisbility = Visibility.Collapsed;
        }

        public void PasswordBoxLostFocus()
        {
            string value = UWPYourNoteLibrary.Util.UserUtilities.IsPasswordCheck(PasswordBoxPassword);
            if (value == null)
            {
                PasswordToolTipContent = "";
                PasswordCheckVisibility = PasswordCheckBorderVisbility = Visibility.Collapsed;
            }
            else
            {
                PasswordToolTipContent = value;
                PasswordCheckVisibility = PasswordCheckBorderVisbility = Visibility.Visible;


            }
        }

        public void RevealModeCheckBoxChecked()
        {

            if (RevealModeCheckBoxIsChecked == false)
            {
                RevealModeCheckBoxIsChecked = true;
                PasswordBoxRevealMode = PasswordRevealMode.Visible;
                ConfirmPasswordBoxRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                RevealModeCheckBoxIsChecked = false;
                PasswordBoxRevealMode = PasswordRevealMode.Hidden;
                ConfirmPasswordBoxRevealMode = PasswordRevealMode.Hidden;
            }
        }

        public void ShowPasswordToolTip()
        {
            if (PasswordCheckVisibility == Visibility.Collapsed)
            {
                PasswordToolTipVisibility = PasswordCheckBorderVisbility = Visibility.Collapsed;
            }
            else
            {
                PasswordToolTipVisibility = PasswordCheckBorderVisbility = Visibility.Visible;
            }
        }

        //------------------------------------------Re Password Box---------------------------------------------------
        public string IsConfirmPasswordCheck(string password, string confirmPassword)
        {
            string value = UWPYourNoteLibrary.Util.UserUtilities.IsPasswordCheck(password);
            if (value == null && password != confirmPassword)
            {
                return "Password and Confirm Password does not match";
            }
            return null;

        }


        //Password Box
        private string _confirmPasswordBoxPassword = "";
        public string ConfirmPasswordBoxPassword
        {
            get { return _confirmPasswordBoxPassword; }
            set
            {
                _confirmPasswordBoxPassword = value;
                ConfirmPasswordBoxLostFocus();
                OnPropertyChanged();
            }
        }


        private PasswordRevealMode _confirmPasswordBoxRevealMode = PasswordRevealMode.Hidden;
        public PasswordRevealMode ConfirmPasswordBoxRevealMode
        {
            get { return _confirmPasswordBoxRevealMode; }
            set
            {
                _confirmPasswordBoxRevealMode = value;
                OnPropertyChanged();
            }
        }



        private string _confirmPasswordBoxPlaceHolderText = "Confirm password";
        public string ConfirmPasswordBoxPlaceHolderText
        {
            get { return _confirmPasswordBoxPlaceHolderText; }
        }


        //Font Icon
        private Visibility _confirmPasswordCheckVisibility = Visibility.Collapsed;
        public Visibility ConfirmPasswordCheckVisibility
        {
            get { return _confirmPasswordCheckVisibility; }
            set
            {
                _confirmPasswordCheckVisibility = value;
                ShowConfirmPasswordToolTip();
                OnPropertyChanged();
            }
        }


        // Tool Tip
        private string _confirmPasswordToolTipContent = "";

        public string ConfirmPasswordToolTipContent
        {
            get { return _confirmPasswordToolTipContent; }
            set
            {
                _confirmPasswordToolTipContent = value;
                OnPropertyChanged();

            }
        }


        private Visibility _confirmPasswordToolTipVisibility = Visibility.Collapsed;

        public Visibility ConfirmPasswordToolTipVisibility
        {
            get { return _confirmPasswordToolTipVisibility; }
            set
            {
                _confirmPasswordToolTipVisibility = value;
                OnPropertyChanged();
            }
        }




        public void ConfirmPasswordBoxPasswordChanged()
        {
            ConfirmPasswordToolTipContent = "";
            ConfirmPasswordCheckVisibility = ConfirmPasswordCheckBorderVisibility = Visibility.Collapsed;
        }

        public void ConfirmPasswordBoxLostFocus()
        {
            string value = IsConfirmPasswordCheck(PasswordBoxPassword, ConfirmPasswordBoxPassword);
            if (value == null)
            {

                ConfirmPasswordToolTipContent = "";
                ConfirmPasswordCheckVisibility = ConfirmPasswordCheckBorderVisibility =  Visibility.Collapsed;
            }
            else
            {
                ConfirmPasswordToolTipContent = value;
                ConfirmPasswordCheckVisibility = ConfirmPasswordCheckBorderVisibility =  Visibility.Visible;

            }
        }

        public void ShowConfirmPasswordToolTip()
        {
            if (ConfirmPasswordCheckVisibility == Visibility.Collapsed)
            {
                ConfirmPasswordToolTipVisibility = ConfirmPasswordCheckBorderVisibility = Visibility.Collapsed;
            }
            else
            {
                ConfirmPasswordToolTipVisibility = ConfirmPasswordCheckBorderVisibility = Visibility.Visible;
            }
        }

        //------------------------------------------- Navigation Buttons-----------------------------------------

        public void CreateAccountClick()
        {
            NameBoxLostFocus();
            EmailBoxLostFocus();
            PasswordBoxLostFocus();
            ConfirmPasswordBoxLostFocus();

            if (NameCheckVisibility == Visibility.Collapsed &&
                EmailCheckVisibility == Visibility.Collapsed &&
                PasswordCheckVisibility == Visibility.Collapsed &&
                ConfirmPasswordCheckVisibility == Visibility.Collapsed)
            {
             
                signUpPageVM.View = this;
                signUpPageVM.check = this;
                signUpPageVM.InsertNewUser(NameBoxContent, EmailBoxContent, PasswordBoxPassword);


            }
        }


        public void NavigateToSignInClick()
        {

            _frame.Navigate(typeof(SignInPage), _frame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });


        }

        public void NavigateTo()
        {
            NavigateToSignInPage();
        }

        private Visibility _nameCheckBorderVisibility = Visibility.Collapsed;

        public Visibility NameCheckBorderVisibility
        {
            get { return _nameCheckBorderVisibility; }
            set { 
                _nameCheckBorderVisibility = value;
                OnPropertyChanged();
            }
        }


        private Visibility _emailCheckBorderVisibility = Visibility.Collapsed;

        public Visibility EmailCheckBorderVisibility
        {
            get { return _emailCheckBorderVisibility; }
            set
            {
                _emailCheckBorderVisibility = value;
                OnPropertyChanged();
            }
        }


        private Visibility _passwordCheckBorderVisbility = Visibility.Collapsed;

        public Visibility PasswordCheckBorderVisbility
        {
            get { return _passwordCheckBorderVisbility; }
            set
            {
                _passwordCheckBorderVisbility = value;
                OnPropertyChanged();
            }
        }


        private Visibility _confirmPasswordCheckBorderVisibility = Visibility.Collapsed;

        public Visibility ConfirmPasswordCheckBorderVisibility
        {
            get { return _confirmPasswordCheckBorderVisibility; }
            set
            {
                _confirmPasswordCheckBorderVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _navigateToSignInContainerVisibility = Visibility.Visible;

        public Visibility NavigateToSignInContainerVisibility
        {
            get { return _navigateToSignInContainerVisibility; }
            set { _navigateToSignInContainerVisibility = value;
                OnPropertyChanged();
            }
        }


       
    }
}
