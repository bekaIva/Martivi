using Martivi.Model;
using Martivi.Pages;
using Martivi.Pages.ContentViews;
using Martivi.Services;
using Martivi.Views.ErrorAndEmpty;
using MartiviSharedLib;
using MartiviSharedLib.Models.Users;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Syncfusion.ListView.XForms;
using Syncfusion.SfPullToRefresh.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Martivi.ViewModels
{
   public class MainViewModel:PropertyChangedBase
    {
        #region Constructors
        public MainViewModel()
        {            
            AppSettings.AddOrUpdateValue("ServerBaseAddress", "http://martivi.net/");
            //AppSettings.AddOrUpdateValue("ServerBaseAddress", "http://192.168.100.11:44379/");
            ConnectCommand = new Command(async () => await Connect());
            DisconnectCommand = new Command(async () => await Disconnect());

            IsConnected = false;

            Instance = this;

            GlobalChatMessages = new ObservableCollection<ChatMessage>();
            ChatMessages = new ObservableCollection<ChatMessage>();
            MadeOrders = new ObservableCollection<Order>();
            SelectedCategory = new Category();
            Services = new ApiServices();
            Categories = new ObservableCollection<Category>();
            Categories.CollectionChanged += (arg1, arg2) =>
            {
                FilterCategory();
            };
            FilteredCategories = new ObservableCollection<Category>();


            AddCommand = new Command<object>(AddQuantity);
            RefreshListing = new Command<object>(async (sender) =>
            {
                var ptf = sender as SfPullToRefresh;
                try
                {
                    ptf.IsRefreshing = true;
                    await GetCategories();
                }
                catch { }
                finally
                {
                    ptf.IsRefreshing = false;
                }
            });
            OrderListCommand = new Command<object>(NavigateOrdersPage);
            CheckoutCommand = new Command(CheckOut);
            RemoveOrderCommand = new Command<object>(RemoveOrder);
            LoadMoreItemsCommand = new Command<object>(LoadMoreItems, CanLoadMoreItems);

            Initialize();



        }

        #endregion



        #region Properties
        private bool _Loaded;

        public bool Loaded
        {
            get { return _Loaded; }
            set { _Loaded = value; OnPropertyChanged(); }
        }

        private ContentView _CategoryView;
        public ContentView CategoryView
        {
            get { return _CategoryView; }
            set { _CategoryView = value; OnPropertyChanged(); }
        }

        private ContentView _ProductView;

        public ContentView ProductView
        {
            get { return _ProductView; }
            set { _ProductView = value; OnPropertyChanged(); }
        }


        private Order _SelectedDetailOrder;

        public Order SelectedDetailOrder
        {
            get { return _SelectedDetailOrder; }
            set 
            {
                _SelectedDetailOrder = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<ChatMessage> _ChatMessages;
        private string newText;
        private string newTextGlobal;
        private string sendIcon;

        public ObservableCollection<ChatMessage> ChatMessages
        {
            get { return _ChatMessages; }
            set { this._ChatMessages = value; }
        }

        private ObservableCollection<ChatMessage> _GlobalChatMessages;

        public ObservableCollection<ChatMessage> GlobalChatMessages
        {
            get { return _GlobalChatMessages; }
            set { _GlobalChatMessages = value; OnPropertyChanged(); }
        }

        private User _CurrentUser;

        public User CurrentUser
        {
            get { return _CurrentUser; }
            set { _CurrentUser = value; OnPropertyChanged(); }
        }



        public string NewText
        {
            get { return newText; }
            set
            {
                newText = value;
                OnPropertyChanged("NewText");
            }
        }
        public string NewTextGlobal
        {
            get { return newTextGlobal; }
            set
            {
                newTextGlobal = value;
                OnPropertyChanged();
            }
        }

        public string SendIcon
        {
            get
            { return sendIcon; }
            set
            { sendIcon = value; }
        }

        public Command<object> SendCommandAdmin { get; set; }
        public Command<object> SendCommandGlobal { get; set; }

        public Command<object> LoadCommand { get; set; }



        private string _Test;

        public string Test
        {
            get { return _Test; }
            set { _Test = value; OnPropertyChanged(); }
        }

        public MainViewModel Instance { get; set; }

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

        private bool _ChatMessagesLoading;

        public bool ChatMessagesLoading
        {
            get { return _ChatMessagesLoading; }
            set { _ChatMessagesLoading = value; OnPropertyChanged(); }
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
        public int UserId
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
        public Command<object> RefreshListing { get; set; }

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
        private ObservableCollection<Category> _FilteredCategories;
        public ObservableCollection<Category> FilteredCategories
        {
            get { return _FilteredCategories; }
            set { _FilteredCategories = value; OnPropertyChanged(); }
        }
        private string _CategoryFilter;

        public string CategoryFilter
        {
            get { return _CategoryFilter ?? string.Empty; }
            set { _CategoryFilter = value; OnPropertyChanged(); FilterCategory(); }
        }


        private ObservableCollection<Order> _MadeOrders;
        public ObservableCollection<Order> MadeOrders
        {
            get { return _MadeOrders; }
            set { _MadeOrders = value; OnPropertyChanged(); }
        }


        public ObservableCollection<Product> Orders { get; set; } = new ObservableCollection<Product>();

        ProductView pv;
        NoItemProductsPage np;
        private Category _SelectedCategory;
        public Category SelectedCategory
        {
            get { return _SelectedCategory; }
            set 
            {
                _SelectedCategory = value;
                OnPropertyChanged();
                if (value?.Products?.Count > 0)
                {
                    if (Loaded)
                    {
                        if (pv == null)
                        {
                            pv = new ProductView();                           
                        }
                        ProductView = pv;
                    }

                }
                else
                {
                    if (Loaded)
                    {
                        if (np == null)
                        {
                            np = new NoItemProductsPage();                           
                        }
                        ProductView = np;
                    }
                }
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

        #region Methods
        public void FilterCategory()
        {
            FilteredCategories = new ObservableCollection<Category>(Categories.Where((c) =>
            {
                if (c.Name.ToLower().Contains(CategoryFilter.ToLower()))
                {
                    return true;
                }
                if (c.Products.Any((p) => { return p.Name.ToLower().Contains(CategoryFilter); })) return true;
                return false;
            }));
        }
        private void InitializeSendCommand()
        {
            SendIcon = "\ue745";
            SendCommandAdmin = new Command<object>(OnSendCommandAdmin);
            SendCommandGlobal = new Command<object>(OnSendGlobalCommand);
            NewText = "";
            newTextGlobal = "";
        }

        private async void OnSendCommandAdmin(object obj)
        {
            try
            {
                if(!IsSignedIn)
                {
                    await Application.Current.MainPage.DisplayAlert("შეცდომა", "გთხოვთ გაიაროთ ავტორიზაცია", "OK");
                    return;
                }
                var Listview = obj as Syncfusion.ListView.XForms.SfListView;
                if (!string.IsNullOrWhiteSpace(NewText))
                {
                    // await hubConnection.InvokeAsync("SendMessage", user, message);

                    var chatMessage = new ChatMessage
                    {
                        UserId = UserId,
                        Username = UserName,
                        Target = MessageTarget.Admin,
                        Text = NewText,
                        TemplateType = TemplateType.OutGoingText,
                        DateTime = string.Format("{0:d/M/yyyy HH:mm:ss}", DateTime.Now)
                    };
                    ChatMessages.Add(chatMessage);
                    hubConnection.InvokeAsync("SendMessage", chatMessage);
                    (Listview.LayoutManager as LinearLayout).ScrollToRowIndex(ChatMessages.Count - 1, Syncfusion.ListView.XForms.ScrollToPosition.Start);
                }
                NewText = null;
            }
            catch (Exception ee)
            {
                await Application.Current.MainPage.DisplayAlert("შეცდომა", ee.Message, "OK");
            }
        }
        private async void OnSendGlobalCommand(object obj)
        {
            try
            {
                if (!IsSignedIn)
                {
                    await Application.Current.MainPage.DisplayAlert("შეცდომა", "გთხოვთ გაიაროთ ავტორიზაცია", "OK");
                    return;
                }
                var Listview = obj as Syncfusion.ListView.XForms.SfListView;
                if (!string.IsNullOrWhiteSpace(NewText))
                {
                    // await hubConnection.InvokeAsync("SendMessage", user, message);

                    var chatMessage = new ChatMessage
                    {
                        UserId = UserId,
                        Username = UserName,
                        Target = MessageTarget.Global,
                        Text = NewText,
                        TemplateType = TemplateType.OutGoingText,
                        DateTime = string.Format("{0:d/M/yyyy HH:mm:ss}", DateTime.Now)
                    };
                    GlobalChatMessages.Add(chatMessage);
                    hubConnection.InvokeAsync("SendMessage", chatMessage);
                    (Listview.LayoutManager as LinearLayout).ScrollToRowIndex(ChatMessages.Count - 1, Syncfusion.ListView.XForms.ScrollToPosition.Start);
                }
                NewText = null;
            }
            catch (Exception ee)
            {
                await Application.Current.MainPage.DisplayAlert("შეცდომა", ee.Message, "OK");
            }
        }
        private void OnLoaded(object obj)
        {
            var ListView = obj as Syncfusion.ListView.XForms.SfListView;
            var scrollView = ListView.Parent as ScrollView;
            ListView.HeightRequest = scrollView.Height;

            if (Device.RuntimePlatform == Device.macOS)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ListView.ScrollTo(2500);
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    (ListView.LayoutManager as LinearLayout).ScrollToRowIndex(this.ChatMessages.Count - 1, Syncfusion.ListView.XForms.ScrollToPosition.Start);
                });
            }
        }
        async Task Connect()
        {
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("JoinChat", UserName);

            if (hubConnection.State == HubConnectionState.Connected) IsConnected = true;
            else IsConnected = false;
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
            if (authRes.Type == UserType.Admin) throw new Exception("გაიარეთ ავტორიზაცია კლიენტის მომხმარებლით");
            Token = authRes.Token;
            UserId = authRes.UserId;
            UserName = authRes.Username;
            FirstName = authRes.FirstName;
            LastName = authRes.LastName;
           
            await LoadUser();
            return authRes;
        }
        internal async void SignOut()
        {
            Token =  null;
            UserId = -1;
            UserName = "Guest";
            FirstName = null;
            LastName = null;
            CurrentUser = null;
            await LoadUser();
        }
        public async Task LoadUser()
        {

            if (Token?.Length > 0 && UserId > 0)
            {
                CurrentUser = await Services.GetUser(UserId, "Bearer " + Token);
                if (CurrentUser == null)
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
                LoadMessages();               
                Connect();
            }
            else
            {
                IsSignedIn = false;
                Disconnect();
            }
        }
        async Task StartHubConnection()
        {

            hubConnection = new HubConnectionBuilder()
      .WithUrl(AppSettings.GetValueOrDefault("ServerBaseAddress", "") + "chatHub", options => { options.AccessTokenProvider = () => Task.FromResult(Token); }).Build();


            hubConnection.On<string>("JoinChat", (user) =>
            {

            });

            hubConnection.On<string>("LeaveChat", (user) =>
            {

            });

            hubConnection.On<ChatMessage>("ReceiveMessage", (message) =>
            {
                switch (message.Target)
                {
                    case MessageTarget.Global:
                        {
                            GlobalChatMessages.Add(message);
                            break;
                        }
                    case MessageTarget.TargetUser:
                        {
                            if (UserId == message.TargetUser)
                            {
                                ChatMessages.Add(message);
                            }
                            break;
                        }
                }
            });
            hubConnection.On("UpdateListing", () =>
            {
                try
                {
                    GetCategories();
                }
                catch
                {

                }

            });
            hubConnection.On("UpdateOrderListing", () =>
            {
                try
                {
                    LoadOrders();
                }
                catch
                {

                }

            });
            hubConnection.Closed += HubConnection_Closed;
        }       

        private async Task HubConnection_Closed(Exception arg)
        {
            

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
        }
        public async Task CancelOrder(Order o)
        {
            try
            {
                if (UpdatingOrdersHistory) return;
                UpdatingOrdersHistory = true;
                await Services.CancelOrder(o, "Bearer " + Token);

            }
            catch
            {

            }
            finally
            {
                UpdatingOrdersHistory = false;
            }            
        }
        public async Task<bool> UpdateUser(UpdateModel update)
        {
           var res = await Services.UpdateUserProfile(update, "Bearer " + Token);
            await LoadUser();
            return res;
        }
        public async Task LoadOrders()
        {
            try
            {
                UpdatingOrdersHistory = true;
              var orders =  await Services.LoadOrders(UserId, "Bearer " + Token);
                Device.BeginInvokeOnMainThread(() =>
                {
                    MadeOrders.Clear();
                    foreach(var order in orders)
                    {
                        if (SelectedDetailOrder != null && SelectedDetailOrder.OrderId == order.OrderId)
                        {
                            SelectedDetailOrder = order;
                        }
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
        public async Task LoadMessages()
        {
            try
            {
                ChatMessagesLoading = true;
                var messages = await Services.GetChatMessages("Bearer " + Token);
                Device.BeginInvokeOnMainThread(() =>
                {
                    ChatMessages.Clear();
                    GlobalChatMessages.Clear();
                    foreach (var message in messages)
                    {
                        if (message.Target == MessageTarget.Global)
                        {
                            GlobalChatMessages.Add(message);
                        }
                        else
                        {
                            ChatMessages.Add(message);
                        }

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
            await StartHubConnection();
            InitializeSendCommand();
            //await Services.SendMessage(new ChatMessage() {Message="ტესტ ტესტ ტეს",UserId=UserId,Side=MessageSide.Client },Token);
            GetCategories();
            LoadUser();
           
        }
       
        private void AddQuantity(object obj)
        {
            var p = obj as Product;
            if (p.QuantityInSupply <= 0) return;
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
                var checkout = await Application.Current.MainPage.DisplayAlert("შეკვეთა", "გსურთ შეკვეთა?", "დიახ", "არა");
                if (checkout)
                {
                    if (!IsSignedIn) throw new Exception("გთხოვთ გაიაროთ ავტორიზაცია!");
                    Order o = new Order() {OrderTimeTicks= DateTime.Now.Ticks, User = new User() { UserId = UserId,Token= "Bearer "+Token }, OrderedProducts = new List<Product>() };
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
            catch(Exception ee)
            {
                await Application.Current.MainPage.DisplayAlert("შეცდომა", ee.Message, "OK");
            }
            finally
            {
                MakingOrder = false;
            }
        }
        CategoryView cv;
        NoItemCategoryPage ncp;
        public async Task GetCategories()
        {
            try
            {
                if (IsBusy) return;
                Categories.Clear();
                IsBusy = true;
                while (!Loaded) await Task.Delay(100);
                if (cv == null) cv = new CategoryView();
                CategoryView =cv;
                var categories = await Services.GetCategories();
                if(categories.Count==0)
                {
                    if (ncp == null) ncp = new NoItemCategoryPage();
                    CategoryView =ncp;
                }
                foreach (var category in categories)
                {
                    if (SelectedCategory?.CategoryId == category.CategoryId) SelectedCategory = category;
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
