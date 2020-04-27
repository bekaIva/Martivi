using Martivi.Model;
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
    public partial class ProductPage : ContentPage
    {
        MainViewModel viewModel;
        public ProductPage()
        {
            try
            {
                InitializeComponent();
                viewModel = this.BindingContext as MainViewModel;
            }
            catch (Exception ee)
            {
                DisplayAlert("Error",ee.Message,"Ok");
            }
        }      
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }
    }
}