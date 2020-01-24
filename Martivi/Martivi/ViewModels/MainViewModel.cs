﻿using Martivi.Model;
using Martivi.Pages;
using Martivi.Services;
using MartiviSharedLib;
using MartiviSharedLib.Models.Users;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private bool _MakingOrder;

        public bool MakingOrder
        {
            get { return _MakingOrder; }
            set { _MakingOrder = value;OnPropertyChanged(); }
        }

        private bool _UpdatingOrdersHistory;

        public bool UpdatingOrdersHistory
        {
            get { return _UpdatingOrdersHistory; }
            set { _UpdatingOrdersHistory = value; OnPropertyChanged(); }
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



        private ObservableCollection<Order> _MadeOrders;
        public ObservableCollection<Order> MadeOrders
        {
            get { return _MadeOrders; }
            set { _MadeOrders = value; OnPropertyChanged(); }
        }


        public ObservableCollection<Product> Orders { get; set; } = new ObservableCollection<Product>();
        private Category _SelectedCategory;

        public Category SelectedCategory
        {
            get { return _SelectedCategory; }
            set { _SelectedCategory = value; }
        }

        private ObservableCollection<MessageModel> _messages;
        public ObservableCollection<MessageModel> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        private string _name;
        private string _message;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }


        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }



        private bool _isConnected;
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        private HubConnection hubConnection;

        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }
        #endregion
        #region Constructors
        public MainViewModel()
        {
            //AppSettings.AddOrUpdateValue("ServerBaseAddress", "https://martiviapi.azurewebsites.net/");
            AppSettings.AddOrUpdateValue("ServerBaseAddress", "http://192.168.100.11:44379/");
            Messages = new ObservableCollection<MessageModel>();
            SendMessageCommand = new Command(async () => { await SendMessage(Name, Message); });
            ConnectCommand = new Command(async () => await Connect());
            DisconnectCommand = new Command(async () => await Disconnect());

            IsConnected = false;

         



            MadeOrders = new ObservableCollection<Order>();
            SelectedCategory = new Category();
            Services = new ApiServices();
            Categories = new ObservableCollection<Category>();
            AddCommand = new Command<object>(AddQuantity);
            OrderListCommand = new Command<object>(NavigateOrdersPage);
            CheckoutCommand = new Command(CheckOut);
            RemoveOrderCommand = new Command<object>(RemoveOrder);
            LoadMoreItemsCommand = new Command<object>(LoadMoreItems, CanLoadMoreItems);
           
            Initialize();
            

        }

        #endregion
        #region Methods
        async Task Connect()
        {
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("JoinChat", UserName);

            IsConnected = true;
        }

        async Task SendMessage(string user, string message)
        {
            await hubConnection.InvokeAsync("SendMessage", user, message);
        }

        async Task Disconnect()
        {
            await hubConnection.InvokeAsync("LeaveChat", Name);
            await hubConnection.StopAsync();

            IsConnected = false;
        }
        public async Task<User> Auth(AuthenticateModelBase auth)
        {
            var authRes = await Services.Authenticate(auth);
            Token = authRes.Token;
            UserId = authRes.UserId;
            UserName = authRes.Username;
            FirstName = authRes.FirstName;
            LastName = authRes.LastName;
           
            await LoadUser();
            return authRes;
        }
        internal async void SingOut()
        {
            Token =  null;
            UserId = -1;
            UserName = "Guest";
            FirstName = null;
            LastName = null;
            await LoadUser();
        }
        public async Task LoadUser()
        {
           
            if (Token?.Length > 0 && UserId > 0)
            {
                var user = await Services.GetUser(UserId, "Bearer "+ Token);
                if (user == null)
                {
                    Token = null; UserId = -1;
                }
            }
            else
            {

            }
            if (Token?.Length > 0 && UserId > 0)
            {
                IsSignedIn = true;
                LoadOrders();
                StartHubConnection();
            }
            else IsSignedIn = false;
        }
        async Task StartHubConnection()
        {
            hubConnection = new HubConnectionBuilder()
      .WithUrl(AppSettings.GetValueOrDefault("ServerBaseAddress", "") + "chatHub", options => { options.AccessTokenProvider = () => Task.FromResult(Token); }).Build();


            hubConnection.On<string>("JoinChat", (user) =>
            {
                Messages.Add(new MessageModel() { User = Name, Message = $"{user} has joined the chat", IsSystemMessage = true });
            });

            hubConnection.On<string>("LeaveChat", (user) =>
            {
                Messages.Add(new MessageModel() { User = Name, Message = $"{user} has left the chat", IsSystemMessage = true });
            });

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Messages.Add(new MessageModel() { User = user, Message = message, IsSystemMessage = false, IsOwnMessage = Name == user });
            });
            Connect();
        }
        public async Task DeleteOrder(Order o)
        {
            try
            {
                if (UpdatingOrdersHistory) return;
                UpdatingOrdersHistory = true;
               await Services.DeleteOrder(o, "Bearer "+ Token);
               
            }
            catch
            {

            }
            finally
            {
                UpdatingOrdersHistory = false;
            }
            LoadOrders();
        }
        public async Task LoadOrders()
        {
            try
            {
                if (UpdatingOrdersHistory) return;
                UpdatingOrdersHistory = true;
              var orders =  await Services.LoadOrders(UserId, "Bearer " + Token);
                Device.BeginInvokeOnMainThread(() =>
                {
                    MadeOrders.Clear();
                    foreach(var order in orders)
                    {
                        MadeOrders.Add(order);
                    }
                });
            }
            catch
            {

            }
            finally
            {
                UpdatingOrdersHistory = false;
            }
        }
        async Task Initialize()
        {

            //await Services.SendMessage(new ChatMessage() {Message="ტესტ ტესტ ტეს",UserId=UserId,Side=MessageSide.Client },Token);
            GetCategories();
            LoadUser();
        }
       
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
            try
            {
                if (MakingOrder) return;
                MakingOrder = true;
                var checkout = await Application.Current.MainPage.DisplayAlert("Checkout", "Do you want to checkout?", "Yes", "No");
                if (checkout)
                {
                    if (!IsSignedIn) throw new Exception("გთხოვთ გაიაროთ ავტორიზაცია!");
                    Order o = new Order() {OrderTimeTicks= DateTime.Now.Ticks, User = new User() { UserId = UserId,Token=Token }, OrderedProducts = new List<Product>() };
                    o.OrderedProducts.AddRange(Orders.ToArray());
                   var res =  await Services.Chekout(o);
                    if (res)
                    {
                        while (Orders.Count > 0)
                        {
                            Orders[Orders.Count - 1].Quantity = 0;
                        }
                        await Application.Current.MainPage.DisplayAlert("", "შეკვეთა წარმატებით შესრულდა!", "OK");
                        LoadOrders();
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Shell.Current.GoToAsync($"//profile/orderstab/orderspage");
                        });
                        return;
                    }
                    await Application.Current.MainPage.DisplayAlert("", "შეკვეთა არ შესრულდა!", "OK");



                }
            }
            catch
            {
                await Application.Current.MainPage.DisplayAlert("", "შეკვეთა არ შესრულდა!", "OK");
            }
            finally
            {
                MakingOrder = false;
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