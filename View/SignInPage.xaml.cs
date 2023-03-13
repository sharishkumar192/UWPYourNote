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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPYourNote.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignInPage : Page, INotifyPropertyChanged
    {
        private Frame _frame;
        public SignInPage()
        {
            this.InitializeComponent();

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _frame = e.Parameter as Frame;
            if (FrequentEmailItemSource == null || FrequentEmailItemSource.Count == 0)
            {
                FrequentEmailBoxVisibility = Visibility.Collapsed;
            }
            else
            {
                FrequentEmailBoxVisibility = Visibility.Visible;
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


        //------------------------------------------Email TextBox---------------------------------------------------

        public static string CheckAlreadyExistingEmail(string email)
        {
            SignInPageViewModel _signInPageViewModel = SignInPageViewModel.SignInPVM;
            if (_signInPageViewModel.IsExistingEmail(email) == true)
                return "An account already exists for this email address";
            return null;
        }

        public string IsEmailCheck(string email)
        {
            string checkNullOrEmpty = UserUtilities.CheckNullOrEmpty(email);

            if (checkNullOrEmpty != null)
                return checkNullOrEmpty;

            string checkValid = UserUtilities.CheckValidEmail(email);


            if (checkValid != null)
                return checkValid;

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
        public ObservableCollection<UWPYourNoteLibrary.Models.User> FrequentEmailItemSource
        {
            get { return _frequentEmailItemSource; }

        }


        public void FrequentEmailItemClick(object sender, ItemClickEventArgs e)
        {
            var frequentUser = (UWPYourNoteLibrary.Models.User)e.ClickedItem;

            EmailBoxContent = frequentUser.userId;
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


        private ObservableCollection<UWPYourNoteLibrary.Models.User> _frequentEmailItemSource = UWPYourNoteLibrary.Models.User.GetFrequentUsers();

        //------------------------------------------- Navigation Buttons-----------------------------------------
        public void LogInButtonClick()
        {
            EmailBoxLostFocus();
            PasswordBoxLostFocus();
            if (EmailCheckVisibility == Visibility.Collapsed &&
                PasswordCheckVisibility == Visibility.Collapsed)
            {
                Tuple<UWPYourNoteLibrary.Models.User, bool> validation = UWPYourNoteLibrary.Models.User.ValidateLogInUser(EmailBoxContent, PasswordBoxPassword);
                if (validation.Item2 == true)
                {

                    Tuple<Frame, UWPYourNoteLibrary.Models.User> tuple = new Tuple<Frame, UWPYourNoteLibrary.Models.User>(_frame, validation.Item1);
                    _frame.Navigate(typeof(HomePage), tuple);

                }
                else
                {
                    PasswordToolTipContent = "Incorrect Email address or password";
                    PasswordCheckVisibility = Visibility.Visible;

                }

            }

        }



        public void NavigateToSignUpClick()
        {
            _frame.Navigate(typeof(SignUpPage), _frame, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
        }

    }
}