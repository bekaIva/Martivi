using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();
        }

        private void FacebookTapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(""));
        }

        private void youtubeTapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(""));
        }

        private void instagramTapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(""));
        }

        private void twitterTapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(""));
        }

     
        private void CallTapped(object sender, EventArgs e)
        {
            PhoneDialer.Open("577220843");
        }
    }
}