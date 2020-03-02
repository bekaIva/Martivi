using Martivi.Model;
using Martivi.ViewModels;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Martivi.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GlobalChatPage : ContentPage
    {
        MainViewModel mv;
        ObservableCollection<ChatMessage> m;
        public GlobalChatPage()
        {
            mv = Application.Current.Resources["MainViewModel"] as MainViewModel;
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            m = listView.ItemsSource as ObservableCollection<ChatMessage>;
            if (m != null)
            {
                m.CollectionChanged += M_CollectionChanged;
            }

            if (Device.RuntimePlatform == Device.macOS)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    listView.ScrollTo(2500);
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    (listView.LayoutManager as LinearLayout).ScrollToRowIndex(m.Count - 1, Syncfusion.ListView.XForms.ScrollToPosition.Start);
                });
            }
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            m.CollectionChanged -= M_CollectionChanged;
        }
        private void M_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (Device.RuntimePlatform == Device.macOS)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        listView.ScrollTo(2500);
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        (listView.LayoutManager as LinearLayout).ScrollToRowIndex(m.Count - 1, Syncfusion.ListView.XForms.ScrollToPosition.Start);
                    });
                }
            }
        }
    }
}