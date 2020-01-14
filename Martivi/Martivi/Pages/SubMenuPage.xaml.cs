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
    public partial class SubMenuPage : ContentPage
    {
        SubMenuViewModel viewModel;
        public SubMenuPage()
        {
            InitializeComponent();
        }
        public SubMenuPage(Model.Menu m)
        {
            InitializeComponent();
            viewModel = this.BindingContext as SubMenuViewModel;
            viewModel.SelectedMenu = m;

            var v = lv.ItemsSource;

        }
    }
}