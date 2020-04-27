using Martivi.Model;
using System.Collections.Generic;
using Xamarin.Forms.Internals;

namespace Martivi.Models.Transaction
{
    public class Coordinate
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
    /// <summary>
    /// Model for review list.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class UserAddress:PropertyChangedBase
    {
        public UserAddress()
        {

        }

        private bool _IsPrimary;

        public bool IsPrimary
        {
            get { return _IsPrimary; }
            set { _IsPrimary = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the property that holds the customer id.
        /// </summary>
        public int UserAddressId { get; set; }

        /// <summary>
        /// Gets or sets the property that has been bound with a label, which displays the customer name.
        /// </summary>
        
        private string _CustomerName;

        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with label, which displays the address type.
        /// </summary>
        private string _AddressType;

        public string AddressType
        {
            get { return _AddressType; }
            set { _AddressType = value; OnPropertyChanged(); }
        }



        private Coordinate _Coordinates;
        public Coordinate Coordinates
        {
            get { return _Coordinates; }
            set { _Coordinates = value; OnPropertyChanged(); }
        }



        /// <summary>
        /// Gets or sets the property that has been bound with label, which displays the customer address.
        /// </summary>
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with label, which displays the customer mobile number.
        /// </summary>

        private string _MobileNumber;

        public string MobileNumber
        {
            get { return _MobileNumber; }
            set { _MobileNumber = value; OnPropertyChanged(); }
        }

    }
}
