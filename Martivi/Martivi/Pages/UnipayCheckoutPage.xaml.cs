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
    public partial class UnipayCheckoutPage : ContentPage
    {
        private bool _Navigating;

        public bool Navigating
        {
            get { return _Navigating; }
            set { _Navigating = value; OnPropertyChanged(); }
        }

        public UnipayCheckoutPage()
        {
            InitializeComponent();
        }

        private void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            Navigating = true;
        }

        private void WebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            Navigating = false;
        }
    }
}