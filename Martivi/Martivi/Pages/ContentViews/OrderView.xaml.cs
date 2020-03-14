using Martivi.ViewModels;
using Martivi.Views.Bookmarks;
using Martivi.Views.Transaction;
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
    public partial class OrderView : ContentView
    {





        MainViewModel ViewModel;
        public OrderView()
        {
            InitializeComponent();
            ViewModel = this.BindingContext as MainViewModel;
        }

        private void OrderClicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new CheckoutPage());
        }

        private void listView_Loaded(object sender, Syncfusion.ListView.XForms.ListViewLoadedEventArgs e)
        {
            try
            {
                var totalH = ViewModel.Orders.Count * 120;
                if (totalH > 360)
                {
                    listView.HeightRequest = 360;
                    return;
                }
                listView.HeightRequest = totalH;

            }
            catch
            {

            }
        }
    }
}