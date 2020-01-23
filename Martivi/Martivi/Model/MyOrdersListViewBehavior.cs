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
            popupLayout.PopupView.HeightRequest = 100;
            popupLayout.PopupView.WidthRequest = 100;
            popupLayout.PopupView.ContentTemplate = new DataTemplate(() =>
            {

                var mainStack = new StackLayout();
                mainStack.BackgroundColor = Color.Teal;

                var deletedButton = new Button()
                {
                    Text = "Delete",
                    HeightRequest = 50,
                    BackgroundColor = Color.Teal,
                    TextColor = Color.White
                };
                deletedButton.Clicked += DeletedButton_Clicked;
                var sortButton = new Button()
                {
                    Text = "Sort",
                    HeightRequest = 50,
                    BackgroundColor = Color.Teal,
                    TextColor = Color.White
                };
                sortButton.Clicked += SortButton_Clicked;
                mainStack.Children.Add(deletedButton);
                mainStack.Children.Add(sortButton);
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

        private void DeletedButton_Clicked(object sender, EventArgs e)
        {

            if (ListView == null)
                return;
            if(item.Status== OrderStatus.Accepted)
            {
                App.Current.MainPage.DisplayAlert("შეცდომა", "სანამ არ დასრულდება შეკვეთა წაშლა არ შეიძლება.", "OK");
                goto cont;
            }
            var source = ListView.ItemsSource as IList<Order>;

            if (source != null && source.Contains(item))
            {
                source.Remove(item);
                mv.DeleteOrder(item);
            }
            else
                App.Current.MainPage.DisplayAlert("Alert", "Unable to delete the item", "OK");

            item = null;
            source = null;

            cont:
            try
            {
                popupLayout.Dismiss();
            }
            catch
            {

            }

        }
    }
}
