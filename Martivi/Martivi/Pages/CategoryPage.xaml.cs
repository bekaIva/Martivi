using Martivi.Services;
using Martivi.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage
    {
        MainViewModel viewModel;
        public CategoryPage()
        {
            InitializeComponent();
            viewModel = this.BindingContext as MainViewModel;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
        }

        private void TryAgainClicked(object sender, EventArgs e)
        {
            viewModel?.GetCategories();
        }
    }
}