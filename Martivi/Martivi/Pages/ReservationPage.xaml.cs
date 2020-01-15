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
    public partial class ReservationPage : ContentPage
    {

        ReservationPageViewModel ViewModel;
        public ReservationPage()
        {
            InitializeComponent();
            ViewModel = this.BindingContext as ReservationPageViewModel;
            
        }


        private async void Reserve_Click(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.Reserve();
            }
            catch
            { }
        }
    }
}