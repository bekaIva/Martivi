using Martivi.Model;
using Martivi.ViewModels;
using MartiviSharedLib;
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
    public partial class SignInPage : ContentPage
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
        MainViewModel vm;

        public SignInPage()
        {
            InitializeComponent();
            vm = BindingContext as MainViewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
           Shell.SetTabBarIsVisible(this, false);
        }

        private void RegistrationTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }

        private async void SigninTapped(object sender, EventArgs e)
        {
            try
            {
                IsBusy = true;
               var user = await vm.Auth(new AuthenticateModelBase() { Username = Email, Password = Password,IsAdmin=false });
               
                    await DisplayAlert("შესრულდა", "მოგესალმებით " + user.FirstName + ", თქვენ წარმატებით გაიარეთ ავტორიზაცია.", "Ok");
                    Navigation.PopAsync();
                    Shell.Current.GoToAsync($"//profile/profiletab/profilepage");
              
            }
            catch(HttpRequestException he)
            {
                DisplayAlert("შეცდომა","კავშირის პრობლემა:\n"+ he.Message, "Ok");
            }
           
            catch (AuthenticateException ee)
            {

                DisplayAlert("შეცდომა", ee.Message, "Ok");
            }
            catch (Exception ee)
            {

                DisplayAlert("შეცდომა", ee.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}