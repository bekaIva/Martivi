using Martivi.Model;
using Martivi.ViewModels;
using Syncfusion.SfPullToRefresh.XForms;
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
    public partial class MyOrdersPage : ContentPage
    {
        MainViewModel vm;
        public MyOrdersPage()
        {
            InitializeComponent();
            vm = BindingContext as MainViewModel;
        }

        private async void refreshing(object sender, EventArgs e)
        {
            var pr = sender as SfPullToRefresh;
            try
            {
               
                pr.IsRefreshing = true;
                await vm.LoadOrders();
            }
            catch
            {

            }
            finally
            {
                pr.IsRefreshing = false;
            }
        }

        private void OrderTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            try
            {
                if (e.ItemData is Order order)
                {
                    
                    Navigation.PushAsync(new OrderDetailPage(order));
                }
            }
            catch (Exception ee)
            {

            }
        }
    }
}