using Martivi.Model;
using Martivi.Models.Transaction;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Martivi.Views.Transaction
{
    /// <summary>
    /// The Delivery view. 
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeliveryView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryView" /> class.
        /// </summary>
        public DeliveryView()
        {
            InitializeComponent();
            geoCoder = new Geocoder();
        }
        Geocoder geoCoder;
        private void AddAddressClicked(object sender, System.EventArgs e)
        {
            AddAddressPopUp.Show();
        }

        private async void map_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
           ((sender as BindableMap).BindingContext as UserAddress).Coordinates = e.Position.Latitude.ToString()+","+e.Position.Longitude.ToString();
            try
            {
                CoordinatesLabel.Text = "(" + e.Position.Latitude.ToString() + " " + e.Position.Longitude.ToString() + ")";
                var approximateLocation = await geoCoder.GetAddressesForPositionAsync(e.Position);
                map.MoveToRegion(new MapSpan(e.Position, 0.1, 0.1));
                var ap = approximateLocation.FirstOrDefault();
                if (ap != string.Empty)
                {
                    CoordinatesLabel.Text += ap;
                }
            }
            catch
            {

            }
            
          
        }

        private async void AddressEntry_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                var approximateLocation = await geoCoder.GetPositionsForAddressAsync(e.NewTextValue);
               var ap=  approximateLocation.FirstOrDefault();
                if(ap!=null)
                {
                    map.MoveToRegion(new MapSpan(new Position(ap.Latitude, ap.Longitude),0.1,0.1));
                }
            }
            catch
            {

            }
        }
    }
}