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
    public partial class OrderPage : ContentPage
    {

        MainViewModel ViewModel;
        public OrderPage()
        {
            InitializeComponent();
            ViewModel = this.BindingContext as MainViewModel;
            
        }


        private async void Reserve_Click(object sender, EventArgs e)
        {
            try
            {
              
            }
            catch
            { }
        }
    }
}