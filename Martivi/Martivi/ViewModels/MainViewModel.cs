using Martivi.Controls;
using Martivi.Model;
using Martivi.Models.Transaction;
using Martivi.Pages;
using Martivi.Pages.ContentViews;
using Martivi.Services;
using Martivi.Views.ErrorAndEmpty;
using MartiviSharedLib;
using MartiviSharedLib.Models.Users;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Syncfusion.ListView.XForms;
using Syncfusion.SfPullToRefresh.XForms;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Martivi.ViewModels
{
   public class MainViewModel:PropertyChangedBase
    {
        #region Constructors
        public MainViewModel()
        {
            AddAddress = new UserAddress();
            AppSettings.AddOrUpdateValue("ServerBaseAddress", "http://martivi.net/");
            //AppSettings.AddOrUpdateValue("ServerBaseAddress", "http://192.168.100.11:44379/");
            AppSettings.AddOrUpdateValue("CheckoutBackLink", AppSettings.GetValueOrDefault("ServerBaseAddress",string.Empty)+ "CheckoutResult");
           
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
            RemoveAddressCommand = new Command<SfButton>(async (arg) =>
            {
                try
                {
                    var res = await Services.RemoveAddress(arg.BindingContext as UserAddress, "Bearer " + Token);
                    LoadAddresses();
                
                }
                catch(Exception e)
                {

                }

            });
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
            RefreshInfo = new Command<object>(async (sender) =>
            {
                var ptf = sender as SfPullToRefresh;
                try
                {
                    ptf.IsRefreshing = true;
                    UpdateInfo();
                }
                catch { }
                finally
                {
                    ptf.IsRefreshing = false;
                }
            });
            OrderListCommand = new Command<object>(NavigateOrdersPage);
            RemoveOrderCommand = new Command<object>(RemoveOrder);
            LoadMoreItemsCommand = new Command<object>(LoadMoreItems, CanLoadMoreItems);
            MinOrder = 30;
            MaxOrder = 100;
            ServiceUnavailable = false;
            Initialize();



        }

        #endregion



        #region Properties
        private UserAddress _AddAddress;

        public UserAddress AddAddress
        {
            get { return _AddAddress; }
            set { _AddAddress = value; OnPropertyChanged(); }
        }
        private bool _Loaded;

        public bool Loaded
        {
            get { return _Loaded; }
            set { _Loaded = value; OnPropertyChanged(); }
        }
        private bool _ServiceUnavailable;

        public bool ServiceUnavailable
        {
            get { return _ServiceUnavailable; }
            set { _ServiceUnavailable = value; OnPropertyChanged(); }
        }

        private double _MinOrder;

        public double MinOrder
        {
            get { return _MinOrder; }
            set { _MinOrder = value; OnPropertyChanged(); }
        }

        private bool _MapClicked;

      

        private double _MaxOrder;

        public double MaxOrder
        {
            get { return _MaxOrder; }
            set { _MaxOrder = value; OnPropertyChanged(); }
        }


        private bool _CategoriesAvailable;

        public bool CategoriesAvailable
        {
            get { return _CategoriesAvailable; }
            set { _CategoriesAvailable = value; OnPropertyChanged(); }
        }

        private bool _NoCategories;

        public bool NoCategories
        {
            get { return _NoCategories; }
            set 
            {
                _NoCategories = value;
                OnPropertyChanged(); 
            }
        }

        private bool _ConnectionError;

        public bool ConnectionError
        {
            get { return _ConnectionError; }
            set { _ConnectionError = value; OnPropertyChanged(); }
        }



        //private ContentView _CategoryView;
        //public ContentView CategoryView
        //{
        //    get { return _CategoryView; }
        //    set { _CategoryView = value; OnPropertyChanged(); }
        //}

        //private ContentView _ProductView;

        //public ContentView ProductView
        //{
        //    get { return _ProductView; }
        //    set { _ProductView = value; OnPropertyChanged(); }
        //}


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

        private Info _Info;
        public Info Info
        {
            get { return _Info; }
            set { _Info = value; OnPropertyChanged(); }
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

        private bool _InfoUpdating;
        public bool InfoUpdating
        {
            get { return _InfoUpdating; }
            set { _InfoUpdating = value; OnPropertyChanged(); }
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
            get 
            {
                return CancelingOrder || DeletingOrder || LoadingOrder || OrderCheckouting || CheckingOrderPaymentStatus;               
            }
            set { _UpdatingOrdersHistory = value; OnPropertyChanged(); }
        }

        private bool _CancelingOrder;

        public bool CancelingOrder
        {
            get { return _CancelingOrder; }
            set { _CancelingOrder = value; OnPropertyChanged();OnPropertyChanged("UpdatingOrdersHistory"); }
        }
        private bool _DeletingOrder;

        public bool DeletingOrder
        {
            get { return _DeletingOrder; }
            set { _DeletingOrder = value; OnPropertyChanged(); OnPropertyChanged("UpdatingOrdersHistory"); }
        }

        private bool _LoadingOrder;

        public bool LoadingOrder
        {
            get { return _LoadingOrder; }
            set { _LoadingOrder = value; OnPropertyChanged(); OnPropertyChanged("UpdatingOrdersHistory"); }
        }

        private bool _OrderCheckouting;

        public bool OrderCheckouting
        {
            get { return _OrderCheckouting; }
            set { _OrderCheckouting = value; OnPropertyChanged(); OnPropertyChanged("UpdatingOrdersHistory"); }
        }

        private bool _CheckingOrderPaymentStatus;

        public bool CheckingOrderPaymentStatus
        {
            get { return _CheckingOrderPaymentStatus; }
            set { _CheckingOrderPaymentStatus = value; OnPropertyChanged(); OnPropertyChanged("UpdatingOrdersHistory"); }
        }


        private bool _ChatMessagesLoading;

        public bool ChatMessagesLoading
        {
            get { return _ChatMessagesLoading; }
            set { _ChatMessagesLoading = value; OnPropertyChanged(); }
        }

        private bool _AddressesLoading;

        public bool AddressesLoading
        {
            get { return _AddressesLoading; }
            set { _AddressesLoading = value; OnPropertyChanged(); }
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
        public Command<object> RefreshInfo { get; set; }
        public Command<SfButton> RemoveAddressCommand { get; set; }
        public Command<object> LoadMoreItemsCommand { get; set; }

        public Command<object> OrderListCommand { get; set; }
        public Command<object> RemoveOrderCommand { get; set; }


        

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

        private Category _SelectedCategory;
        public Category SelectedCategory
        {
            get { return _SelectedCategory; }
            set 
            {
                _SelectedCategory = value;
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
            if (hubConnection.State == HubConnectionState.Connected) Disconnect();
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
            Token = null;
            UserId = -1;
            UserName = "Guest";
            FirstName = null;
            LastName = null;
            CurrentUser = null;
            IsSignedIn = false;
            Disconnect();
            //await LoadUser();
        }
        public async Task LoadUser()
        {

            if (Token?.Length > 0 && UserId > 0)
            {
                try
                {
                    CurrentUser = await Services.GetUser(UserId, "Bearer " + Token);
                    if (CurrentUser == null)
                    {
                        SignOut();

                    }
                }
                catch
                {
                    SignOut();
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
                LoadAddresses();
                Connect();
            }
            else
            {
                SignOut();
            }
        }
        async Task StartHubConnection()
        {

            hubConnection = new HubConnectionBuilder()
      .WithUrl(AppSettings.GetValueOrDefault("ServerBaseAddress", "") + "chatHub", options => { options.AccessTokenProvider = () => Task.FromResult(Token); }).WithAutomaticReconnect(new RetryPolicy()).Build();



            hubConnection.On<ChatMessage>("ReceiveMessage", (message) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (message.Target)
                    {
                        case MessageTarget.Global:
                            {
                                try
                                {
                                    var stream = GetStreamFromFile("stairs.mp3");
                                    var audio = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                                    audio.Load(stream);
                                    audio.Play();
                                    GlobalChatMessages.Add(message);
                                }
                                catch
                                {

                                }
                                
                                break;
                            }
                        case MessageTarget.TargetUser:
                            {
                                if (UserId == message.TargetUser)
                                {
                                    try
                                    {
                                        var stream = GetStreamFromFile("stairs.mp3");
                                        var audio = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                                        audio.Load(stream);
                                        audio.Play();
                                        ChatMessages.Add(message);
                                    }
                                    catch
                                    {

                                    }
                                    
                                }
                                break;
                            }
                    }
                });
                
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
            hubConnection.On("UpdateSettings", () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        UpdateSettings();
                    }
                    catch
                    {

                    }
                });
              

            });
            hubConnection.On<string>("JoinChat", (user) =>
            {

            });
            hubConnection.On("UpdateInfo", () =>
            {
                try
                {
                    UpdateInfo();
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
            hubConnection.On<System.Text.Json.JsonElement>("UpdateOrder", (o) =>
            {
                try
                {
                    var str = o.GetRawText();
                    var order = JsonConvert.DeserializeObject<Order>(str);
                    for (int i = 0; i < MadeOrders.Count; i++)
                    {
                        if (MadeOrders[i].OrderId == order.OrderId)
                        {
                            MadeOrders[i].OrderTimeTicks = order.OrderTimeTicks;
                            MadeOrders[i].Payment = order.Payment;
                            MadeOrders[i].Status = order.Status;
                            MadeOrders[i].IsSeen = order.IsSeen;
                        }
                    }
                }
                catch (Exception ee)
                {

                }

            });
            hubConnection.Closed += HubConnection_Closed;
            
            
        }       

        private async Task HubConnection_Closed(Exception arg)
        {
            

        }
        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("Martivi.Sounds." + filename);

            return stream;
        }
        public async Task DeleteOrder(Order o)
        {
            try
            {
                if (DeletingOrder) return;
                DeletingOrder = true;
               await Services.DeleteOrder(o, "Bearer "+ Token);
               
            }
            catch
            {
                
            }
            finally
            {
                DeletingOrder = false;
            }
        }
        public async Task CancelOrder(Order o)
        {
            try
            {
                if (CancelingOrder) return;
                CancelingOrder = true;
                await Services.CancelOrder(o, "Bearer " + Token);

            }
            catch
            {

            }
            finally
            {
                CancelingOrder = false;
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
                LoadingOrder = true;
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
                LoadingOrder = false;
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
                ChatMessagesLoading = false;
            }
        }
        public async Task<bool> AddUserAdress()
        {
            if (string.IsNullOrEmpty(AddAddress.CustomerName)) throw new Exception("სახელი არ არის მითითებული");
            if (string.IsNullOrEmpty(AddAddress.Address)) throw new Exception("მისამართი არ არის მითითებული");
            if (string.IsNullOrEmpty(AddAddress.MobileNumber)) throw new Exception("ტელეფონი არ არის მითითებული");
            if (AddAddress.Coordinates== null)
            {
                var WithoutMap = await Application.Current.MainPage.DisplayAlert("შეტყობინება", "მისამართი რუკაზე არ არის მითითებული, გსურთ რუკის გარეშე დამატება?", "კი", "არა");
                if(WithoutMap)
                {

                }
                else
                {
                    return false;
                }
                
            }
            var res = await Services.AddAddress(AddAddress, "Bearer " + Token);
            LoadAddresses();
            return res;
        }
        
        public async Task LoadAddresses()
        {
            try
            {
                AddressesLoading = true;
                var addresses = await Services.GetAddresses("Bearer " + Token);
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (CurrentUser == null) return;
                    if (CurrentUser.UserAddresses == null) CurrentUser.UserAddresses = new ObservableCollection<UserAddress>();
                    CurrentUser.UserAddresses.Clear();
                    foreach (var address in addresses)
                    {
                        CurrentUser.UserAddresses.Add(address);
                    }
                });
            }
            catch
            {

            }
            finally
            {
                AddressesLoading = false;
            }
        }
        async Task Initialize()
        {
            await StartHubConnection();
            InitializeSendCommand();
            //await Services.SendMessage(new ChatMessage() {Message="ტესტ ტესტ ტეს",UserId=UserId,Side=MessageSide.Client },Token);
            GetCategories();
            LoadUser();
            UpdateInfo();
            UpdateSettings();
        }
       

       

       
        public async Task UpdateSettings()
        {
            try
            {
                var settings = await Services.GetSettings();
                foreach (var setting in settings)
                {
                    try
                    {
                        switch (setting.Name)
                        {
                            case "MinOrder":
                                {
                                    MinOrder = Convert.ToDouble(setting.Value);
                                    break;
                                };
                            case "MaxOrder":
                                {
                                    MaxOrder = Convert.ToDouble(setting.Value);
                                    break;
                                };
                            case "ServiceUnavailable":
                                {
                                    ServiceUnavailable = Convert.ToBoolean(setting.Value);
                                    break;
                                };
                        }
                    }
                    catch{}
                }

            }
            catch
            {

            }
            finally
            {
            }
        }
        public async Task UpdateInfo()
        {
            try
            {
                InfoUpdating = true;
                Info = await Services.GetAboutInfo();
               
            }
            catch
            {

            }
            finally
            {
                InfoUpdating = false;
            }
        }
        private void AddQuantity(object obj)
        {
            var p = obj as Product;
            if (p.QuantityInSupply <= 0) return;
            p.Quantity += 1;
        }
        private void NavigateOrdersPage(object obj)
        {
            var ordersPage = new OrderPage();
            Shell.Current.Navigation.PushAsync(ordersPage);
        }
        private void RemoveOrder(object obj)
        {
            var p = obj as Product;
            p.Quantity = 0;
        }
        public async Task Checkout(Order o)
        {
            try
            {
                if (OrderCheckouting) throw new Exception("გთხოვთ დაიცადოთ სანამ მიმდინარე გადახდის პროცესი შესრულდება.");
                OrderCheckouting = true;
                var res = await Services.Checkout(o, "Bearer " + Token);
                await Application.Current.MainPage.Navigation.PushAsync(new UnipayCheckoutPage() { BindingContext = res });
            }
            catch (Exception ee)
            {

                throw ee;
            }
            finally { OrderCheckouting = false; }
            //go to checkout
        }
        public async void MakeOrder(bool PayNow)
        {
            try
            {
                if (MakingOrder) return;
                MakingOrder = true;
                if (TotalPrice > MaxOrder) throw new Exception(string.Format("შეკვეთის მაქსიმალური ღირებულება უნდა იყოს {0}₾",MaxOrder));
                if (TotalPrice < MinOrder) throw new Exception(string.Format("შეკვეთის მინიმალური ღირებულება უნდა იყოს {0}₾", MinOrder));
                if (ServiceUnavailable) throw new Exception("შეკვეთების მიღება დროებით შეჩერებულია");
                var checkout = await Application.Current.MainPage.DisplayAlert("შეკვეთა", "გსურთ შეკვეთა?", "დიახ", "არა");
                if (checkout)
                {
                    if (!IsSignedIn) throw new Exception("გთხოვთ გაიაროთ ავტორიზაცია!");
                    var SelectedAddress = CurrentUser?.UserAddresses?.FirstOrDefault(a => a.IsPrimary);
                    if (SelectedAddress == null) throw new Exception("გთხოვთ აირჩიოთ მისამართი");
                   
                    Order o = new Order() {Payment= PaymentStatus.NotPaid, IsSeen=false, OrderAddress=SelectedAddress, OrderTimeTicks= DateTime.Now.Ticks, User= CurrentUser,  OrderedProducts = new List<Product>() };
                    o.OrderedProducts.AddRange(Orders.ToArray());
                    o =  await Services.MakeOrder(o,"Bearer " + Token);
                    if (o!=null)
                    {
                        LoadOrders();
                        while (Orders.Count > 0)
                        {
                            Orders[Orders.Count - 1].Quantity = 0;
                        }
                        if(PayNow)
                        {
                           await Checkout(o);
                            
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("", "შეკვეთა წარმატებით შესრულდა!", "OK");
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Shell.Current.GoToAsync($"//home/orderstab/orderspage");
                            });
                        }
                       
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
        public async Task GetCategories()
        {
            try
            {
                if (IsBusy) return;                
                IsBusy = true;
                CategoriesAvailable = true;
                ConnectionError = false;
                NoCategories = false;
                var categories = await Services.GetCategories();
                Categories.Clear();
                if (categories.Count==0)
                {
                    CategoriesAvailable = false;
                    NoCategories = true;
                }
                else
                {
                    CategoriesAvailable = true;
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
                CategoriesAvailable = false;
                ConnectionError = true;
                NoCategories = false;
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
