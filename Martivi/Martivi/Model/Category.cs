﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Martivi.Model
{
    public class Category:PropertyChangedBase
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        private ObservableCollection<Product> _Products;

        public ObservableCollection<Product> Products
        {
            get { return _Products; }
            set { _Products = value; OnPropertyChanged(); }
        }

    }
    public class Product:PropertyChangedBase
    {
        public int CategoryId { get; set; }

        public int ProductId { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Weight { get; set; }

        public double Price { get; set; }


        private int quantity = 0;
        private double totalPrice = 0;
        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    TotalPrice = quantity * Price;
                    OnPropertyChanged();
                }
            }
        }
        private int _QuantityInSupply;

        public int QuantityInSupply
        {
            get { return _QuantityInSupply; }
            set { _QuantityInSupply = value; OnPropertyChanged(); }
        }


        public double TotalPrice
        {
            get { return totalPrice; }
            set
            {
                if (totalPrice != value)
                {
                    totalPrice = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
