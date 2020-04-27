using Martivi.Model;
using Martivi.Models.Transaction;
using Martivi.Pages;
using Martivi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
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

        public DeliveryView()
        {
            InitializeComponent();
            
        }
        private void AddAddressClicked(object sender, System.EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new AddAddressPage());
            }
            catch (System.Exception ee)
            {

                throw;
            }
        }

       
    }
}