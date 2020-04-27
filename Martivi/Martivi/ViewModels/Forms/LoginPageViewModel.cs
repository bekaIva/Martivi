using Martivi.Model;
using Martivi.Views.Forms;
using MartiviSharedLib;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Martivi.ViewModels.Forms
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class LoginPageViewModel : LoginViewModel
    {
        #region Fields

        private string password;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="LoginPageViewModel" /> class.
        /// </summary>
        public LoginPageViewModel()
        {
            this.LoginCommand = new Command(this.LoginClicked);
            this.SignUpCommand = new Command(this.SignUpClicked);
            this.ForgotPasswordCommand = new Command(this.ForgotPasswordClicked);
            this.SocialMediaLoginCommand = new Command(this.SocialLoggedIn);
        }

        #endregion

        #region property
        private bool _IsBusy;

        public bool IsBusy
        {
            get { return _IsBusy; }
            set { _IsBusy = value; NotifyPropertyChanged(); }
        }

        private SimpleLoginPage _LoginPage;

        public SimpleLoginPage LoginPage
        {
            get { return _LoginPage; }
            set { _LoginPage = value; NotifyPropertyChanged(); }
        }

        private MainViewModel _ViewModel;

        public MainViewModel ViewModel
        {
            get { return _ViewModel; }
            set { _ViewModel = value; NotifyPropertyChanged(); }
        }


        /// <summary>
        /// Gets or sets the property that is bound with an entry that gets the password from user in the login page.
        /// </summary>
        public string Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.password = value;
                this.NotifyPropertyChanged();
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that is executed when the Log In button is clicked.
        /// </summary>
        public Command LoginCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Sign Up button is clicked.
        /// </summary>
        public Command SignUpCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the Forgot Password button is clicked.
        /// </summary>
        public Command ForgotPasswordCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that is executed when the social media login button is clicked.
        /// </summary>
        public Command SocialMediaLoginCommand { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Invoked when the Log In button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void LoginClicked(object obj)
        {
            try
            {
                IsBusy = true;
                if (string.IsNullOrEmpty(Email)) throw new Exception("გთხოვთ შეიყვანოთ მომხმარებელი");
                if (string.IsNullOrEmpty(Password)) throw new Exception("გთხოვთ შეიყვანოთ პაროლი");
                var user = await ViewModel.Auth(new AuthenticateModelBase() { Username = Email, Password = Password, IsAdmin = false });

                await LoginPage.DisplayAlert("შესრულდა", "მოგესალმებით " + user.FirstName + ", თქვენ წარმატებით გაიარეთ ავტორიზაცია.", "Ok");
                LoginPage.Navigation.PopAsync();
                Shell.Current.GoToAsync($"//profile/profiletab/profilepage");

            }
            catch (HttpRequestException he)
            {
                LoginPage.DisplayAlert("შეცდომა", "კავშირის პრობლემა:\n" + he.Message, "Ok");
            }

            catch (AuthenticateException ee)
            {

                LoginPage.DisplayAlert("შეცდომა", ee.Message, "Ok");
            }
            catch (Exception ee)
            {

                LoginPage.DisplayAlert("შეცდომა", ee.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SignUpClicked(object obj)
        {
            try
            {
                LoginPage.Navigation.PushAsync(new SimpleSignUpPage());
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Invoked when the Forgot Password button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void ForgotPasswordClicked(object obj)
        {
            var label = obj as Label;
            label.BackgroundColor = Color.FromHex("#70FFFFFF");
            await Task.Delay(100);
            label.BackgroundColor = Color.Transparent;
            LoginPage.Navigation.PushAsync(new PasswordRecoverPage());
        }

        /// <summary>
        /// Invoked when social media login button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void SocialLoggedIn(object obj)
        {
            // Do something
        }

        #endregion
    }
}