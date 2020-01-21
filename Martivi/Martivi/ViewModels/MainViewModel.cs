using Martivi.Model;
using Martivi.Pages;
using Martivi.Services;
using MartiviSharedLib;
using MartiviSharedLib.Models.Users;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Martivi.ViewModels
{
   public class MainViewModel:PropertyChangedBase
    {
        #region Properties

        private string _Test;

        public string Test
        {
            get { return _Test; }
            set { _Test = value; OnPropertyChanged(); }
        }


        private bool _IsSignedIn;

        public bool IsSignedIn
        {
            get { return _IsSignedIn; }
            set { _IsSignedIn = value; OnPropertyChanged(); }
        }


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
        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { _IsBusy = value; OnPropertyChanged(); }
        }
        private static ISettings AppSettings =>
    CrossSettings.Current;

        public string UserName
        {
            get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty);
            set
            {

                AppSettings.AddOrUpdateValue(nameof(UserName), value);
                OnPropertyChanged();
            }
        }
        public static int UserId
        {
            get => AppSettings.GetValueOrDefault(nameof(UserId), -1);
            set => AppSettings.AddOrUpdateValue(nameof(UserId), value);
        }

        public string Token
        {
            get => AppSettings.GetValueOrDefault(nameof(Token), string.Empty);
            set 
            {
                AppSettings.AddOrUpdateValue(nameof(Token), value);
                OnPropertyChanged();
            }
        }
        public string FirstName
        {
            get => AppSettings.GetValueOrDefault(nameof(FirstName), string.Empty);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(FirstName), value);
                OnPropertyChanged();
            }
        }
        public string LastName
        {
            get => AppSettings.GetValueOrDefault(nameof(LastName), string.Empty);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(LastName), value);
                OnPropertyChanged();
            }
        }

        public Command<object> AddCommand { get; set; }

        public Command<object> LoadMoreItemsCommand { get; set; }

        public Command<object> OrderListCommand { get; set; }

        public Command<object> RemoveOrderCommand { get; set; }


        public Command CheckoutCommand { get; set; }

        public ApiServices Services { get; set; }
        private ObservableCollection<Category> _Categories;
        public ObservableCollection<Category> Categories
        {
            get { return _Categories; }
            set { _Categories = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Product> Orders { get; set; } = new ObservableCollection<Product>();
        private Category _SelectedCategory;

        public Category SelectedCategory
        {
            get { return _SelectedCategory; }
            set { _SelectedCategory = value; }
        }

        #endregion
        #region Constructors
        public MainViewModel()
        {
            SelectedCategory = new Category();
            Services = new ApiServices();
            Categories = new ObservableCollection<Category>();
            AddCommand = new Command<object>(AddQuantity);
            OrderListCommand = new Command<object>(NavigateOrdersPage);
            CheckoutCommand = new Command(CheckOut);
            RemoveOrderCommand = new Command<object>(RemoveOrder);
            LoadMoreItemsCommand = new Command<object>(LoadMoreItems, CanLoadMoreItems);
            AppSettings.AddOrUpdateValue("ServerBaseAddress", "https://martiviapi.azurewebsites.net/");
            Initialize();
            

        }
        public async Task Auth(AuthenticateModelBase auth)
        {
           var authRes = await Services.Authenticate(auth);
            Token = "Bearer "+ authRes.Token;
            UserId = authRes.UserId;
            UserName = authRes.Username;
            FirstName = authRes.FirstName;
            LastName = authRes.LastName;
            await LoadUser();
        }
        public async Task LoadUser()
        {
            if (Token?.Length > 0 && UserId > 0)
            {
               var user= await Services.GetUser(UserId, Token);
            }
            else
            {
                UserName = "Guest";
            }
        }
        async Task Initialize()
        {
          
           //await Services.SendMessage(new ChatMessage() {Message="ტესტ ტესტ ტეს",UserId=UserId,Side=MessageSide.Client },Token);
            GetCategories();
        }
        #endregion
        #region Methods
        private void AddQuantity(object obj)
        {
            var p = obj as Product;
            p.Quantity += 1;
        }
        private void NavigateOrdersPage(object obj)
        {
            var productsView = obj as Page;
            var ordersPage = new OrderPage();
            productsView.Navigation.PushAsync(ordersPage);
        }
        private void RemoveOrder(object obj)
        {
            var p = obj as Product;
            p.Quantity = 0;
        }
       
        private async void CheckOut(object obj)
        {
            var checkout = await Application.Current.MainPage.DisplayAlert("Checkout", "Do you want to checkout?", "Yes", "No");
            if (checkout)
            {
                while (Orders.Count > 0)
                {
                    Orders[Orders.Count - 1].Quantity = 0;
                }

                await Application.Current.MainPage.DisplayAlert("", "Your order has been placed.", "OK");

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (obj != null)
                        (obj as ContentPage).Navigation.PopAsync();
                });
            }
        }

        public async Task GetCategories()
        {
            try
            {
                if (IsBusy) return;
                IsBusy = true;
                var categories = await Services.GetCategories();
                foreach (var category in categories)
                {
                    foreach(var p in category.Products)
                    {
                        p.PropertyChanged += (s,e) => 
                        {
                            var product = s as Product;
                            if (e.PropertyName == "Quantity")
                            {
                                if (Orders.Contains(product) && product.Quantity <= 0)
                                    Orders.Remove(product);
                                else if (!Orders.Contains(product) && product.Quantity > 0)
                                    Orders.Add(product);

                                TotalOrderedItems = Orders.Count;
                                TotalPrice = 0;
                                for (int j = 0; j < Orders.Count; j++)
                                {
                                    var order = Orders[j];
                                    TotalPrice += order.TotalPrice;
                                }
                            }
                        };
                    }
                    Categories.Add(category);
                }
                IsBusy = false;
            }
            catch(Exception e)
            {
                IsBusy = false;
            }
           
        }
        private async void LoadMoreItems(object obj)
        {
            var listview = obj as Syncfusion.ListView.XForms.SfListView;
            listview.IsBusy = true;
            //await GetCategories();

            listview.IsBusy = false;
        }
        private bool CanLoadMoreItems(object obj)
        {
           return false;
        }
        #endregion
    }
}
