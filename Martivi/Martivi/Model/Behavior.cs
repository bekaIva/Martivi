#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Martivi.ViewModels;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Expander;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Martivi.Model
{
    [Preserve(AllMembers = true)]

    #region DataTemplateSelectorBehavior
    public class MyExpander:SfExpander
    {
        public MyExpander()
        {
            
        }
        public void changeProperty(string propName)
        {
            OnPropertyChanged(propName);
        }
    }
    public class DataTemplateSelectorBehavior : Behavior<Syncfusion.ListView.XForms.SfListView>
    {
        #region Fields

        private MainViewModel DataTemplateSelectorViewModel;
        private Syncfusion.ListView.XForms.SfListView ListView;
        private ScrollView scrollView;

        #endregion

        #region Overrides
        protected override void OnAttachedTo(Syncfusion.ListView.XForms.SfListView bindable)
        {
            ListView = bindable;
            base.OnAttachedTo(bindable);
            DataTemplateSelectorViewModel = (MainViewModel)ListView.BindingContext;
            ListView.Loaded += ListView_Loaded;
        }

        private void ListView_Loaded(object sender, ListViewLoadedEventArgs e)
        {
            scrollView = ListView.Parent as ScrollView;
            ListView.HeightRequest = scrollView.Height;

            if (Device.RuntimePlatform == Device.macOS)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ListView.ScrollTo(2500);
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    (ListView.LayoutManager as LinearLayout).ScrollToRowIndex(DataTemplateSelectorViewModel.ChatMessages.Count - 1, Syncfusion.ListView.XForms.ScrollToPosition.Start);
                });
            }
        }

        protected override void OnDetachingFrom(Syncfusion.ListView.XForms.SfListView bindable)
        {
            ListView = null;
            base.OnDetachingFrom(bindable);
        }

        #endregion

    }
    public class AnimatedItemListViewBehaviours : Behavior<SfListView>

    {

        private SfListView listView;
        
        protected override void OnAttachedTo(BindableObject bindable)

        {

            listView = bindable as SfListView;
          
            listView.ItemGenerator = new ItemGeneratorExt(listView);

            base.OnAttachedTo(bindable);

        }

        
    }
    public class ItemGeneratorExt : ItemGenerator

    {

        public SfListView ListView { get; set; }

        public ItemGeneratorExt(SfListView listview) : base(listview)

        {
            
            ListView = listview;

        }

        protected override ListViewItem OnCreateListViewItem(int itemIndex, ItemType type, object data = null)

        {

            if (type == ItemType.Record)

                return new ListViewItemExt(ListView);

            return base.OnCreateListViewItem(itemIndex, type, data);

        }

    }




    public class ListViewItemExt : ListViewItem

    {

        private SfListView _listView;



        public ListViewItemExt(SfListView listView)

        {

            _listView = listView;

        }
        
        protected override void OnItemAppearing()
        {
            this.Opacity = 0;
            TranslationY = -15;
            this.FadeTo(1, 400, Easing.CubicOut);


            this.TranslateTo(0, 0, 400, easing: Easing.CubicOut);

            base.OnItemAppearing();

        }

    }

    #endregion
}
