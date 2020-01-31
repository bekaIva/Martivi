using Martivi.Model;
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
    public partial class OrderDetailPage : ContentPage
    {
        private int totalOrderedItems = 0;
        private double totalPrice = 0;
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
        public double TotalPrice
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

        public OrderDetailPage(Order order)
        {
            Order = order;
            InitializeComponent();
            TotalOrderedItems = order.OrderedProducts.Count;
            foreach(var p in order.OrderedProducts)
            {
                TotalPrice += p.TotalPrice;
            }
        }
    }
}