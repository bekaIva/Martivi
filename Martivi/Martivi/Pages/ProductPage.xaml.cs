﻿using Martivi.Model;
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
    public partial class ProductPage : ContentPage
    {
        MainViewModel viewModel;
        public ProductPage()
        {
            InitializeComponent();
            viewModel = this.BindingContext as MainViewModel;
        }

      
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }
    }
}