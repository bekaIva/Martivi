using Martivi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi.Pages.ContentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderView : ContentView
    {
        MainViewModel ViewModel;
        public OrderView()
        {
            InitializeComponent();
            ViewModel = this.BindingContext as MainViewModel;
        }
    }
}