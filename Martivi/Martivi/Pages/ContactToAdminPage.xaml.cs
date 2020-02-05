using Martivi.ViewModels;
using Syncfusion.ListView.XForms;
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
    public partial class ContactToAdminPage : ContentPage
    {
        MainViewModel mv;
        public ContactToAdminPage()
        {
            mv = Application.Current.Resources["MainViewModel"] as MainViewModel;
            InitializeComponent();
        }

        private void ItemAppearing(object sender, Syncfusion.ListView.XForms.ItemAppearingEventArgs e)
        {
            (listView.LayoutManager as LinearLayout).ScrollToRowIndex(mv.ChatMessages.Count);
        }
    }
}