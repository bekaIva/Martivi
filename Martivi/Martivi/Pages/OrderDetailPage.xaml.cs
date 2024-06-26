﻿using Martivi.Model;
using Martivi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailPage : ContentPage
    {
        private int totalOrderedItems = 0;
        private decimal totalPrice = 0;
        public int TotalOrderedItems
        {
            get { return totalOrderedItems; }
            set
            {
                if (totalOrderedItems != value)
                {
                    totalOrderedItems = value;
                    OnPropertyChanged();
                }
            }
        }
        public decimal TotalPrice
        {
            get { return totalPrice; }
            set
            {
                if (totalPrice != value)
                {
                    totalPrice = value;
                    OnPropertyChanged();
                }
            }
        }

        private Order _Order;

        public Order Order
        {
            get { return _Order; }
            set { _Order = value; OnPropertyChanged(); }
        }
        MainViewModel mv;
        public OrderDetailPage(Order order)
        {
            try
            {
               
                mv = Application.Current.Resources["MainViewModel"] as MainViewModel;
                Order = order;
                this.BindingContext = order;
                InitializeComponent();
                TotalOrderedItems = Order.OrderedProducts.Count;
                foreach (var p in Order.OrderedProducts)
                {
                    TotalPrice += p.TotalPrice;
                }
            }
            catch (Exception e)
            {

            }
        }

        private void stepProgress_ChildAdded(object sender, ElementEventArgs e)
        {

        }

        //private void listView_Loaded(object sender, Syncfusion.ListView.XForms.ListViewLoadedEventArgs e)
        //{
        //    try
        //    {
        //        var totalH = mv.SelectedDetailOrder.OrderedProducts.Count * 140;
        //        if (totalH > 560)
        //        {
        //            listView.HeightRequest = 560 + 300;
        //            return;
        //        }
        //        listView.HeightRequest = totalH + 300;

        //    }
        //    catch
        //    {

        //    }
        //}

        private void UserFrameTapped(object sender, EventArgs e)
        {

        }

        private async void PayClicked(object sender, EventArgs e)
        {
            try
            {
               await mv.Checkout(Order);
            }
            catch(Exception ee)
            {
                DisplayAlert("შეცდომა", ee.Message,"Ok");
            }
        }

        private void CopyClicked(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetTextAsync(CoordinateSpan.Text);
                DependencyService.Get<IAlertNative>().ShowToast("Copied!");
            }
            catch
            {

            }
        }
    }
}