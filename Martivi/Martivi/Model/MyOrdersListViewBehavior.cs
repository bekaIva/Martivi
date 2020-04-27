using Martivi.ViewModels;
using Syncfusion.DataSource;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Martivi.Model
{
    public class Behavior : Behavior<SfListView>
    {
        MainViewModel mv;
        SfListView ListView;
        int sortorder = 0;
        Order item;
        SfPopupLayout popupLayout;
        protected override void OnAttachedTo(SfListView listView)
        {
            ListView = listView;
            mv = ListView.BindingContext as MainViewModel;
            ListView.ItemHolding += ListView_ItemHolding;
            ListView.ScrollStateChanged += ListView_ScrollStateChanged;
            ListView.ItemTapped += ListView_ItemTapped;
            base.OnAttachedTo(listView);
        }

        private void ListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            try
            {
                popupLayout.Dismiss();
            }
            catch
            {

            }

        }

        private void ListView_ScrollStateChanged(object sender, ScrollStateChangedEventArgs e)
        {
            try
            {
                popupLayout.Dismiss();
            }
            catch
            {

            }

        }

        private void ListView_ItemHolding(object sender, ItemHoldingEventArgs e)
        {
            item = e.ItemData as Order;
            popupLayout = new SfPopupLayout();

            popupLayout.PopupView.WidthRequest = 100;
            popupLayout.PopupView.ContentTemplate = new DataTemplate(() =>
            {

                var mainStack = new StackLayout();

                var deletedButton = new Button()
                {
                    Text = "წაშლა",
                    HeightRequest = 50,
                    BackgroundColor = Color.White,
                    TextColor = Color.Black
                };
                deletedButton.Clicked += DeletedButton_Clicked;
                var sortButton = new Button()
                {
                    Text = "დალაგება",
                    HeightRequest = 50,
                    BackgroundColor = Color.White,
                    TextColor = Color.Black
                };
                sortButton.Clicked += SortButton_Clicked;

                

                    var PayButton = new Button()
                    {
                        Text = "გადახდა",
                        HeightRequest = 50,
                        BackgroundColor = Color.White,
                        TextColor = Color.Black
                    };
                    PayButton.Clicked += PayButton_Clicked;
                    mainStack.Children.Add(PayButton);
                

                
                
                if (item.Status == OrderStatus.Accepted)
                {                    
                    var CancelButton = new Button()
                    {
                        Text = "გაუქმება",
                        HeightRequest = 50,
                        BackgroundColor = Color.White,
                        TextColor = Color.Black
                    };
                    CancelButton.Clicked += CancelButton_Clicked;
                    mainStack.Children.Add(CancelButton);
                }

                mainStack.Children.Add(deletedButton);
                mainStack.Children.Add(sortButton);

                popupLayout.PopupView.HeightRequest = mainStack.Children.Count*50+10;

                return mainStack;

            });
            popupLayout.PopupView.ShowHeader = false;
            popupLayout.PopupView.ShowFooter = false;
            if (e.Position.Y + 100 <= ListView.Height && e.Position.X + 100 > ListView.Width)
                popupLayout.Show((double)(e.Position.X - 100), (double)(e.Position.Y));
            else if (e.Position.Y + 100 > ListView.Height && e.Position.X + 100 < ListView.Width)
                popupLayout.Show((double)e.Position.X, (double)(e.Position.Y - 100));
            else if (e.Position.Y + 100 > ListView.Height && e.Position.X + 100 > ListView.Width)
                popupLayout.Show((double)(e.Position.X - 100), (double)(e.Position.Y - 100));
            else
                popupLayout.Show((double)e.Position.X, (double)(e.Position.Y));
        }

        private async void PayButton_Clicked(object sender, EventArgs e)
        {
            if (ListView == null)
                return;
            try { popupLayout.Dismiss(); } catch { }
            try
            {


                await mv.Checkout(item);


                popupLayout.Dismiss();
            }
            catch (Exception ee)
            {
                await App.Current.MainPage.DisplayAlert("", ee.Message, "Ok");
            }

        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            if (ListView == null)
                return;
            try { popupLayout.Dismiss(); } catch { }
            if (item.Status == OrderStatus.Accepted)
            {
                if (await App.Current.MainPage.DisplayAlert("", "დარწმუნებული ხართ, რომ გსურთ შეკვეთის გაუქმება?", "კი", "არა"))
                {
                    mv.CancelOrder(item);
                }
            }
         
        }

        private void SortButton_Clicked(object sender, EventArgs e)
        {
            if (ListView == null)
                return;

            ListView.DataSource.SortDescriptors.Clear();
            popupLayout.Dismiss();
            ListView.DataSource.LiveDataUpdateMode = LiveDataUpdateMode.AllowDataShaping;
            if (sortorder == 0)
            {
                ListView.DataSource.SortDescriptors.Add(new SortDescriptor { PropertyName = "OrderId", Direction = ListSortDirection.Descending });
                sortorder = 1;
            }
            else
            {
                ListView.DataSource.SortDescriptors.Add(new SortDescriptor { PropertyName = "OrderId", Direction = ListSortDirection.Ascending });
                sortorder = 0;
            }
        }

        private void Dismiss()
        {
            popupLayout.IsVisible = false;
        }

        private async void DeletedButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (ListView == null)
                    return;
                try { popupLayout.Dismiss(); } catch { }
                if (item.Status == OrderStatus.Accepted)
                {

                    if (await App.Current.MainPage.DisplayAlert("", "შეკვეთა შესრულების პროცესშია, დარწმუნებული ხართ, რომ გსურთ გაუქმება და წაშლა?.", "კი", "არა"))
                    {
                        await mv.CancelOrder(item);
                    }
                    else
                    {
                        return;
                    }
                }
                var source = ListView.ItemsSource as IList<Order>;

                if (source != null && source.Contains(item))
                {
                   await mv.DeleteOrder(item);
                }
                else
                    App.Current.MainPage.DisplayAlert("Alert", "Unable to delete the item", "OK");

                source = null;

            }
            catch (Exception ee)
            {
                App.Current.MainPage.DisplayAlert("შეცდომა", ee.Message, "OK");
            }
         

        }
    }
}
