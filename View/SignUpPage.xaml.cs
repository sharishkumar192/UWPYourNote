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
using YourNoteUWP.Util;
using YourNoteUWP.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YourNoteUWP.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignUpPage : Page, INotifyPropertyChanged
    {
        private SignUpPageViewModel _signUpPageViewModel;
        private Frame _frame;

        public SignUpPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _frame = e.Parameter as Frame;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //------------------------------------------Name TextBox---------------------------------------------------

        public string IsNameCheck(string check)
        {
            string checkNullOrEmpty = UserUtilities.CheckNullOrEmpty(check);
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
            NameCheckVisibility = Visibility.Collapsed;
        }

        public void NameBoxLostFocus()
        {

            string value = IsNameCheck(NameBoxContent);
            if (value == null)
            {
                NameToolTipContent = "";
                NameCheckVisibility = Visibility.Collapsed;

            }
            else
            {
                NameToolTipContent = value;
                NameCheckVisibility = Visibility.Visible;

            }
        }

        public void ShowNameToolTip()
        {
            if (NameCheckVisibility == Visibility.Collapsed)
            {
                NameToolTipVisibility = Visibility.Collapsed;
            }
            else
            {
                NameToolTipVisibility = Visibility.Visible;
            }
        }

        //------------------------------------------Email TextBox---------------------------------------------------

        public static string CheckAlreadyExistingEmail(string email)
        {
            SignInPageViewModel _signInPageViewModel = SignInPageViewModel.SignInPVM;
            if (_signInPageViewModel.IsExistingEmail(email) == true)
                return "An account already exists for this email address";
            return null;
        }
        private string IsEmailCheck(string email)
        {
            string checkNullOrEmpty = UserUtilities.CheckNullOrEmpty(email);

            if (checkNullOrEmpty != null)
                return checkNullOrEmpty;

            string checkValid = UserUtilities.CheckValidEmail(email);


            if (checkValid != null)
                return checkValid;

            string existedMail = CheckAlreadyExistingEmail(email);


            if (existedMail != null)
                return existedMail;


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
            PasswordCheckVisibility = Visibility.Collapsed;
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
                PasswordToolTipVisibility = Visibility.Collapsed;
            }
            else
            {
                PasswordToolTipVisibility = Visibility.Visible;
            }
        }

        //------------------------------------------Re Password Box---------------------------------------------------
        public string IsConfirmPasswordCheck(string password, string confirmPassword)
        {
            string value = UserUtilities.IsPasswordCheck(password);
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
            ConfirmPasswordCheckVisibility = Visibility.Collapsed;
        }

        public void ConfirmPasswordBoxLostFocus()
        {
            string value = IsConfirmPasswordCheck(PasswordBoxPassword, ConfirmPasswordBoxPassword);
            if (value == null)
            {

                ConfirmPasswordToolTipContent = "";
                ConfirmPasswordCheckVisibility = Visibility.Collapsed;
            }
            else
            {
                ConfirmPasswordToolTipContent = value;
                ConfirmPasswordCheckVisibility = Visibility.Visible;

            }
        }

        public void ShowConfirmPasswordToolTip()
        {
            if (ConfirmPasswordCheckVisibility == Visibility.Collapsed)
            {
                ConfirmPasswordToolTipVisibility = Visibility.Collapsed;
            }
            else
            {
                ConfirmPasswordToolTipVisibility = Visibility.Visible;
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
                _signUpPageViewModel = SignUpPageViewModel.SignUpPVM;
                _signUpPageViewModel.InsertNewUser(NameBoxContent, EmailBoxContent, PasswordBoxPassword);
                   _frame.Navigate(typeof(SignInPage), _frame);

            }
        }
        public void NavigateToSignInClick()
        {

            _frame.Navigate(typeof(SignInPage), _frame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });


        }
    }
}
