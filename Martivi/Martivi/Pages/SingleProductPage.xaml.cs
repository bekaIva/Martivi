using Martivi.Model;
using Martivi.ViewModels;
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
    public partial class SingleProductPage : ContentPage
    {
        public SingleProductPage(MainViewModel vm,Product p)
        {
            this.BindingContext = vm;
            
            InitializeComponent();
            contentGrid.BindingContext = p;
        }
    }
}