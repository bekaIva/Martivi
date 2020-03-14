using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Martivi.Models.Transaction;
using Martivi.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Linq;
using Martivi.Model;

namespace Martivi.ViewModels.Transaction
{
    /// <summary>
    /// ViewModel for Checkout page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class CheckoutPageViewModel : BaseViewModel
    {
        #region Fields

     

        private ObservableCollection<Payment> paymentModes;


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="CheckoutPageViewModel" /> class.
        /// </summary>
        public CheckoutPageViewModel()
        {
           

            this.PaymentModes = new ObservableCollection<Payment>
            {
                new Payment {PaymentMode = "Debit / Credit Card"},
                new Payment {PaymentMode = "გადახდა მიწოდებისას"}
            };

           


           
            this.PlaceOrderCommand = new Command(this.PlaceOrderClicked);
            this.PaymentOptionCommand = new Command(PaymentOptionClicked);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the property that has been bound with SfListView, which displays the delivery address.
        /// </summary>
      

        private MainViewModel _MainViewModel;

        public MainViewModel MainViewModel
        {
            get { return _MainViewModel; }
            set { _MainViewModel = value; NotifyPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with SfListView, which displays the payment modes.
        /// </summary>
        public ObservableCollection<Payment> PaymentModes
        {
            get { return this.paymentModes; }

            set
            {
                if (this.paymentModes == value)
                {
                    return;
                }

                this.paymentModes = value;
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the property that has been bound with a list view, which displays the cart details.
        /// </summary>
     

        /// <summary>
        /// Gets or sets the property that has been bound with a label, which displays total price.
        /// </summary>
      

        #endregion

        #region Command

       


        /// <summary>
        /// Gets or sets the command that will be executed when the Edit button is clicked.
        /// </summary>
        public Command PlaceOrderCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the payment option button is clicked.
        /// </summary>
        public Command PaymentOptionCommand { get; set; }

       

        #endregion

        #region Methods

       
        private async void PlaceOrderClicked(object obj)
        {
            var pm = paymentModes.FirstOrDefault(p => p.IsChecked);
            if (pm == null)
            {
                await Application.Current.MainPage.DisplayAlert("", "გთხოვთ აირჩიოთ გადახდის მეთოდი", "Cancel");
                return;
            }
            switch(pm.PaymentMode)
            {
                case "Debit / Credit Card":
                    {

                        break;
                    }
                case "გადახდა მიწოდებისას":
                    {
                        MainViewModel.CheckOut(PaymentStatus.NotPaid);
                        break;
                    }


            }
        }

        private void PaymentOptionClicked(object obj)
        {
            if (obj is RowDefinition rowDefinition && rowDefinition.Height.Value == 0)
            {
                rowDefinition.Height = GridLength.Auto;
            }
        }

      

        #endregion
    }
}