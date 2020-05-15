using Martivi.ViewModels;
using Martivi.Views.Bookmarks;
using Martivi.Views.Transaction;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Expander;
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

        private async void RemoveTapped(object sender, EventArgs e)
        {
            try
            {
                await Task.Delay(200);
                sfExpander.changeProperty("Content");

            }
            catch 
            {

            }
            //try
            //{
            //    expander.IsExpanded = !expander.IsExpanded;
            //    expander.IsExpanded = !expander.IsExpanded;
            //}
            //catch 
            //{

            //}
        }

        //private void listView_Loaded(object sender, Syncfusion.ListView.XForms.ListViewLoadedEventArgs e)
        //{
        //    try
        //    {
        //        var totalH = ViewModel.Orders.Count * 150;
        //        if (totalH > 390)
        //        {
        //            listView.HeightRequest = 390;
        //            return;
        //        }
        //        listView.HeightRequest = totalH;

        //    }
        //    catch
        //    {

        //    }
        //}
    }
}