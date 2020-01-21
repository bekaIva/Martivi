using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
           Shell.SetTabBarIsVisible(this, false);
        }
        private void Button_Clicked(object sender, EventArgs e)
        {

            Shell.Current.Navigation.PushAsync(new HomePage());
        }

        private void RegistrationTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignUpPage());
        }
    }
}