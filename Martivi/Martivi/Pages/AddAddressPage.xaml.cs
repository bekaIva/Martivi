using Martivi.Controls;
using Martivi.Model;
using Martivi.Models.Transaction;
using Martivi.ViewModels;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Martivi.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAddressPage : ContentPage
    {

        MainViewModel mv;
        Geocoder geoCoder;

        private Command<object> _SearchPositionReturnCommand;

        public Command<object> SearchPositionReturnCommand
        {
            get { return _SearchPositionReturnCommand; }
            set { _SearchPositionReturnCommand = value; OnPropertyChanged(); }
        }


        public AddAddressPage()
        {
            InitializeComponent();
            mv = this.BindingContext as MainViewModel;
            geoCoder = new Geocoder();
            SearchPositionReturnCommand = new Command<object>((obj) =>
            {
                SetPos();
            });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                if (mv.AddAddress?.Coordinates == null)
                {
                    CurrentPosition();
                }
            }
            catch { }
        }
        public async Task CurrentPosition()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var location = await locator.GetPositionAsync(TimeSpan.FromTicks(10000));
                Position position = new Position(location.Latitude, location.Longitude);
                var pos = new MapSpan(position, 0.1, 0.1);
                if (mv.AddAddress?.Coordinates == null)
                {
                    viewMap.MoveToRegion(pos);
                }
            }
            catch (Exception ee)
            {
            }
        }
        private void AddMappClicked(object sender, MapClickedEventArgs e)
        {
            try
            {
                mv.AddAddress.Coordinates = new Coordinate() { Latitude = e.Position.Latitude, Longitude = e.Position.Longitude };

            }
            catch
            {
            }
        }
      
        private async void AddClicked(object sender, EventArgs e)
        {
            try
            {

                var res = await mv.AddUserAdress();
                if (res)
                {
                    this.Navigation.PopAsync();
                }
                else
                {
                    throw new Exception("მისამართის დამატება არ მოხდა");
                }
            }
            catch (Exception ee)
            {
                DisplayAlert("შეცდომა", ee.Message, "Ok");
            }
        }

        CancellationTokenSource ts;
        private async void SearchPositionEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetPos();
        }
        void SetPos()
        {
            try
            {
                ts?.Cancel();
                ts = new CancellationTokenSource();
                SearchPosition(SearchPositionEntry.Text, ts.Token);
            }
            catch
            {

            }
        }
        private bool _Searching;

        public bool Searching
        {
            get { return _Searching; }
            set { _Searching = value; OnPropertyChanged(); }
        }

        public async Task SearchPosition(string posName, CancellationToken token)
        {
            try
            {
                while (Searching)
                {
                    await Task.Delay(200);
                    if (token.IsCancellationRequested) return;
                }
                Searching = true;
                var approximateLocation = await geoCoder.GetPositionsForAddressAsync(posName);
                token.ThrowIfCancellationRequested();
                var ap = approximateLocation.FirstOrDefault();
                if (ap != null && ap.Longitude != 0 && ap.Latitude != 0)
                {
                    token.ThrowIfCancellationRequested();
                    viewMap.MoveToRegion(new MapSpan(ap, 0.1, 0.1));
                    mv.AddAddress.Coordinates = new Coordinate() { Latitude=ap.Latitude,Longitude=ap.Longitude};
                    //viewMap.MoveToRegion(new MapSpan(new Position(ap.Latitude, ap.Longitude), 0.1, 0.1));
                }
                Searching = false;
            }
            catch
            {
                Searching = false;
            }
        }

        private void SwitchMapTypeClicked(object sender, EventArgs e)
        {
            switch (viewMap.MapType)
            {
                case MapType.Street:viewMap.MapType = MapType.Satellite;
                    break;
                case MapType.Satellite:
                    viewMap.MapType = MapType.Hybrid;
                    break;
                case MapType.Hybrid:
                    viewMap.MapType = MapType.Street;
                    break;
                default:
                    break;
            }
        }
    }
}