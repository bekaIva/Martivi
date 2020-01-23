using Martivi.ViewModels;
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
        }

        private void SignInOutClicked(object sender, EventArgs e)
        {
            try
            {
                if (!mv.IsSignedIn)
                {
                    Shell.Current.FlyoutIsPresented = false;
                    var page = new SignInPage();
                    Shell.Current.Navigation.PushAsync(page);
                }
                else
                {
                    mv.SingOut();
                }
            }
            catch(Exception ee)
            {
                DisplayAlert("შეცდომა", ee.Message, "Ok");
            }
            
        }

        
    }
}