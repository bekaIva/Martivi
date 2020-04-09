using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using Martivi.Pages;
using Martivi.ViewModels;

namespace Martivi.Model
{

    public class CategorySearchHandler : SearchHandler
    {
        public MainViewModel ViewModel { get; set; }
        public Page CurrentPage { get; set; }
        long lastTick;
        public CategorySearchHandler() : base()
        {
            ItemsSource = new ObservableCollection<object>();
        }
        protected override async void OnQueryChanged(string oldValue, string newValue)
        {
            try
            {
                base.OnQueryChanged(oldValue, newValue);
                if (DateTime.Now.Ticks - lastTick > 3000)
                {
                    lastTick = DateTime.Now.Ticks;
                    List<object> src = new List<object>();
                    //.Where((c) => { if ((c.Name?.ToLower().Contains(newValue) ?? false) ||
                    //(c.Name?.Contains(newValue) ?? false)) return true; else return false; })

                    (ItemsSource as ObservableCollection<object>)?.Clear();
                    var l = ViewModel.Categories.SelectMany((c) =>
                    {
                        List<object> r = new List<object>();
                        if (c.Name?.ToLower().Contains(newValue.ToLower()) ?? false) r.Add(c);
                        r.AddRange(c.Products.Where(p => p.Name?.ToLower().Contains(newValue.ToLower()) ?? false));
                        return r;
                    });
                    foreach (var o in l) (ItemsSource as ObservableCollection<object>)?.Add(o);

                    //ViewModel.SearchQuick(ts.Token).ContinueWith((res) =>
                    //{
                    //    ViewModel.BeginInvokeOnMainThreadAsync(() =>
                    //    {
                    //        this.ItemsSource = res.Result;
                    //    });
                    //});

                }

            }
            catch
            {
                lastTick = DateTime.Now.Ticks;
            }
            finally { }
        }
        protected override void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            if (item is Category c)
            {
                ViewModel.SelectedCategory = c;
                CurrentPage.Navigation.PushAsync(new ProductPage());
            }
            if(item is Product p)
            {
               CurrentPage.Navigation.PushAsync(new SingleProductPage(ViewModel, p));
            }

        }
        protected override async void OnQueryConfirmed()
        {
            try
            {
                base.OnQueryConfirmed();


            }
            catch (Exception ee)
            {

            }
        }
    }
    public class ProductSearchHandler : SearchHandler
    {
        public MainViewModel ViewModel { get; set; }
        public Page CurrentPage { get; set; }
        long lastTick;
        public ProductSearchHandler() : base()
        {
            ItemsSource = new ObservableCollection<object>();
        }
        protected override async void OnQueryChanged(string oldValue, string newValue)
        {
            try
            {
                base.OnQueryChanged(oldValue, newValue);
                if (DateTime.Now.Ticks - lastTick > 3000)
                {
                    lastTick = DateTime.Now.Ticks;
                    ItemsSource = ViewModel.SelectedCategory.FilteredProducts;
                }
            }
            catch
            {
                lastTick = DateTime.Now.Ticks;
            }
            finally { }
        }
        protected override void OnItemSelected(object item)
        {
            base.OnItemSelected(item);
            if (item is Product p)
            {
                CurrentPage.Navigation.PushAsync(new SingleProductPage(ViewModel, p));
            }

        }
        protected override async void OnQueryConfirmed()
        {
            try
            {
                base.OnQueryConfirmed();


            }
            catch (Exception ee)
            {

            }
        }
    }
}
