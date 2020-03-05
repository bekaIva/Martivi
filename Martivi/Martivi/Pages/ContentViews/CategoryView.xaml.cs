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
    public partial class CategoryView : ContentView
    {
        MainViewModel viewModel;
        public CategoryView()
        {
            InitializeComponent();
            viewModel = this.BindingContext as MainViewModel;
        }
   

        private void lv_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.SelectedCategory = e.Item as Category;
            Navigation.PushAsync(new ProductPage());
        }
    }
}