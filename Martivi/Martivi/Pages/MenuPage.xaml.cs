using Martivi.Services;
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
    public partial class MenuPage : ContentPage
    {
        

        
       

        public MenuPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
          
        }

        private void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var Selected = e.SelectedItem as Model.Menu;
            Navigation.PushAsync(new SubMenuPage(Selected));
        }
    }
}