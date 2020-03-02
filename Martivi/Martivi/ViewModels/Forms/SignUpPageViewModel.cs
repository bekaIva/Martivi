using Martivi.Model;
using Martivi.Views.Forms;
using MartiviSharedLib.Models.Users;
using System;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Martivi.ViewModels.Forms
{
    /// <summary>
    /// ViewModel for sign-up page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SignUpPageViewModel : LoginViewModel
    {
        #region Fields

        private string name;

        private string password;

        private string confirmPassword;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="SignUpPageViewModel" /> class.
        /// </summary>
        public SignUpPageViewModel()
        {
            this.LoginCommand = new Command(this.LoginClicked);
            this.SignUpCommand = new Command(this.SignUpClicked);
        }

        #endregion

        #region Property
        private bool _IsBusy;

        public bool IsBusy
        {
            get { return _IsBusy; }
            set { _IsBusy = value; NotifyPropertyChanged(); }
        }

        private SimpleSignUpPage _SignUpPage;

        public SimpleSignUpPage SignUpPage
        {
            get { return _SignUpPage; }
            set { _SignUpPage = value; NotifyPropertyChanged(); }
        }

        private MainViewModel _ViewModel;

        public MainViewModel ViewModel
        {
            get { return _ViewModel; }
            set { _ViewModel = value; NotifyPropertyChanged(); }
        }
        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the name from user in the Sign Up page.
        /// </summary>
      

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (this.name == value)
                {
                    return;
                }

                this.name = value;
                this.NotifyPropertyChanged();
            }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; NotifyPropertyChanged(); }
        }

        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; NotifyPropertyChanged(); }
        }

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { _Address = value; NotifyPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password from users in the Sign Up page.
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

        /// <summary>
        /// Gets or sets the property that bounds with an entry that gets the password confirmation from users in the Sign Up page.
        /// </summary>
        

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

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Log in button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void LoginClicked(object obj)
        {
            SignUpPage.Navigation.PushAsync(new SimpleLoginPage());
        }
        void Checkvalidity()
        {
            if (!IsValidEmail(Email)) throw new ValidityException("ელ-ფოსტის ფორმატი არასწორია!");
            if (!(Password?.Length > 0)) throw new ValidityException("პაროლი არ არის შეყვანილი!");
            if (!(Name?.Length > 0)) throw new ValidityException("სახელი არ არის შეყვანილი!");
            if (!(LastName?.Length > 0)) throw new ValidityException("გვარი არ არის შეყვანილი!");
            if (!(Phone?.Length > 0)) throw new ValidityException("ტელეფონი არ არის შეყვანილი!");
            if (!(Address?.Length > 0)) throw new ValidityException("მისამართი არ არის შეყვანილი!");
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                var host = addr.Host;
                string[] domain = host.Split('.');
                if (domain.Count() == 1)
                {
                    return false;
                }
                else
                {
                    if (domain.Last().Length < 2)
                    {
                        return false;
                    }
                }
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Invoked when the Sign Up button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void SignUpClicked(object obj)
        {
            try
            {
                IsBusy = true;
                Checkvalidity();
                var res = await ViewModel.Services.Register(new RegisterModelBase { FirstName = Name, LastName = LastName, Password = Password, Phone = Phone, UserAddress = Address, Username = Email });
                if (res.UserId > 0)
                {
                    await SignUpPage.DisplayAlert("შესრულდა", res.FirstName + ", თქვენ წარმატებით გაიარეთ რეგისტრაცია. გთხოვთ გაიაროთ ავტორიზაცია.", "Ok");
                    SignUpPage.Navigation.PopAsync();
                }
                else
                {
                    throw new RegistrationException("რეგისტრაცია არ შესრულდა!");
                }
            }
            catch (HttpRequestException he)
            {
                SignUpPage.DisplayAlert("შეცდომა", "კავშირის პრობლემა:\n" + he.Message, "Ok");
            }
            catch (RegistrationException re)
            {
                SignUpPage.DisplayAlert("შეცდომა", re.Message, "Ok");
            }
            catch (ValidityException ve)
            {
                SignUpPage.DisplayAlert("შეცდომა", ve.Message, "Ok");
            }
            catch (Exception ee)
            {
                SignUpPage.DisplayAlert("შეცდომა", ee.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}