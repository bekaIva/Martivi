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
    public partial class CategoryView : ContentView
    {
        MainViewModel viewModel;
        public CategoryView()
        {
            InitializeComponent();
            viewModel = this.BindingContext as MainViewModel;
        }
        

        private async void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (viewModel?.SelectedCategory != null)
            {
                Navigation.PushAsync(new ProductPage());
            }
             
        }
    }
}