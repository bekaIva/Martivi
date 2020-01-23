using Martivi.Model;
using Martivi.ViewModels;
using MartiviSharedLib.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private bool _IsBusy;

        public bool IsBusy
        {
            get { return _IsBusy; }
            set { _IsBusy = value; OnPropertyChanged(); }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set { _Email = value; OnPropertyChanged(); }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged(); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }

        private string _LastName;
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; OnPropertyChanged(); }
        }

        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; OnPropertyChanged(); }
        }

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { _Address = value; OnPropertyChanged(); }
        }

        MainViewModel vm;
        public SignUpPage()
        {
            InitializeComponent();
            vm = BindingContext as MainViewModel;
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
            Shell.SetTabBarIsVisible(this, false);
        }

        private async void RegistrationClicked(object sender, EventArgs e)
        {
            try
            {
                IsBusy = true;
                Checkvalidity();
               var res = await vm.Services.Register(new RegisterModelBase { FirstName = Name, LastName = LastName, Password = Password, Phone = Phone, UserAddress = Address, Username = Email });
                if(res.UserId>0)
                {
                    await DisplayAlert("შესრულდა", res.FirstName + ", თქვენ წარმატებით გაიარეთ რეგისტრაცია. გთხოვთ გაიაროთ ავტორიზაცია.", "Ok");
                    Navigation.PopAsync();
                }
                else
                {
                    throw new RegistrationException("რეგისტრაცია არ შესრულდა!");
                }
            }
            catch (HttpRequestException he)
            {
                DisplayAlert("შეცდომა", "კავშირის პრობლემა:\n" + he.Message, "Ok");
            }
            catch (RegistrationException re)
            {
                DisplayAlert("შეცდომა", re.Message, "Ok");
            }
            catch (ValidityException ve)
            {
                DisplayAlert("შეცდომა", ve.Message, "Ok");
            }
            catch(Exception ee)
            {
                DisplayAlert("შეცდომა", ee.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
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
    }
}