using Martivi.ViewModels;
using Martivi.Views.Forms;
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
    public partial class MainShell : Shell
    {
        MainViewModel mv;
        Random r = new Random();
        public MainShell()
        {
            InitializeComponent();
            mv = Application.Current.Resources["MainViewModel"] as MainViewModel;
            mv.Loaded = true;
        }

        private void SignInOutClicked(object sender, EventArgs e)
        {
            try
            {
                if (!mv.IsSignedIn)
                {
                    Shell.Current.FlyoutIsPresented = false;
                    var page = new SimpleLoginPage();
                    Shell.Current.Navigation.PushAsync(page);
                }
                else
                {
                    mv.SignOut();
                    mv.LoadUser();
                }
            }
            catch(Exception ee)
            {
                DisplayAlert("შეცდომა", ee.Message, "Ok");
            }
            
        }

        private void MenuItemTapped(object sender, EventArgs e)
        {
            try
            {
                if (!mv.IsSignedIn)
                {
                    DependencyService.Get<IAlertNative>().ShowToast("გთხოვთ გაიაროთ ავტორიზაცია");
                }

    }
            catch 
            {

            }
        }
    }
}