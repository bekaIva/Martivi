using Martivi.Model;
using Martivi.ViewModels;
using Syncfusion.XForms.PopupLayout;
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
            Title = p.Name;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            popupLayout.Show(true);
            //SfPopupLayout pl = new SfPopupLayout();
            //pl.PopupView.ContentTemplate = new DataTemplate(()=> 
            //{
            //    Grid g = new Grid();
            //    g.BindingContext = contentGrid.BindingContext;
            //    Image i = new Image();
            //    i.SetBinding(Image.SourceProperty, "Image");
            //    g.Children.Add(i);
            //    return g;
            //});
            //pl.PopupView.IsFullScreen = true;
            //pl.Show(true);

        }

        private void FulscreenImageTapped(object sender, EventArgs e)
        {
            popupLayout.Dismiss();
        }
    }
}