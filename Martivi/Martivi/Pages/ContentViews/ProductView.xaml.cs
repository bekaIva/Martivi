using Martivi.Model;
using Martivi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi.Pages.ContentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductView : ContentView
    {
        MainViewModel viewModel;
        public ProductView()
        {
            InitializeComponent();
            viewModel = this.BindingContext as MainViewModel;
        }
        

        private void PorductTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new SingleProductPage(viewModel, e.ItemData as Product));
        }
       
    }
}