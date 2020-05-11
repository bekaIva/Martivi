#region Copyright Syncfusion Inc. 2001-2019.
// Copyright Syncfusion Inc. 2001-2019. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Martivi.Models.Transaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;

namespace Martivi.Model
{
    [Preserve(AllMembers = true)]
   
    public class PaymentStatusToTextOrColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter is string s&&value is PaymentStatus ps)
            {
                switch(s)
                {
                    case "Visibility":
                        {
                            if (ps == PaymentStatus.NotPaid) return true;
                            else return false;
                        }
                    case "Color":
                        {
                            switch (ps)
                            {
                                case PaymentStatus.NotPaid: return Color.FromRgb(184, 32, 32);

                                

                                default: return Color.Gray;
                            }
                            break;
                        }
                    case "Text":
                        {
                            switch (ps)
                            {
                                case PaymentStatus.NotPaid: return "გადაუხდელია";

                                default: return value;
                            }
                        }
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class CoordinateToPin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Coordinate c)
            {
                Position pos = new Position(c.Latitude, c.Longitude);
                Pin p = new Pin() { Position = pos, Label = "ადგილმდებარეობა" };
                return p;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class CoordinateToMapSpan : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Coordinate c)
            {
                MapSpan ms = new MapSpan(new Position(c.Latitude, c.Longitude), 0.1, 0.1);
                return ms;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
    public class CoordinateToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Coordinate c)
            {
                return c.Latitude.ToString() + ", " + c.Longitude.ToString();
            }




            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class OrderStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           if(value is OrderStatus os)
            {
                switch(os)
                {
                    case OrderStatus.Accepted: return "შესრულების მოლოდინში" ;
                    case OrderStatus.Completed: return "შეკვეთა შესრულებულია";
                    case OrderStatus.Canceled: return "შეკვეთა გაუქმებულია";

                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OrderedProductsToTotalPriceCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double totalPrice=0;
            if(value is List<Product> products)
            {
                foreach(var p in products)
                {
                    totalPrice += p.Price * p.Quantity;
                }
            }
            return string.Format("₾{0:0.00}", totalPrice);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsSignedInConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(parameter is string p)
            {
                switch(p)
                {
                    case "Label":
                        {
                            if(value is bool v)
                            {
                                if (v == true)
                                {
                                    return "გამოსვლა";
                                }
                                else
                                {
                                    return "შესვლა";
                                }
                            }
                            break;
                        }
                    case "Icon":
                        {
                            if (value is bool v)
                            {
                                if (v) return Application.Current.Resources["SignOutImageSource"];
                                else return Application.Current.Resources["SignInImageSource"];
                            }
                                
                            break;
                        }
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ShortDescription : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count;

            if (value is string desc && parameter is string scount && int.TryParse(scount, out count) && count < desc.Length)
            {
                return new string(desc.Take(count).ToArray()) + "...";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InverseZeroVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && (int)value > 0)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [Preserve(AllMembers = true)]
    public class ZeroVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && (int)value <= 0)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }    

    [Preserve(AllMembers = true)]
    public class TotalItemsCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && (int)value <= 1)
                return value + " პროდუქტი |";
            return value + " პროდუქტები |";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [Preserve(AllMembers = true)]
    public class CurrencyFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var d = (double)value;
            return string.Format("₾{0:0.00}", d);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CountToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool retVal;
            if (value is int v && v > 0) retVal= true;
            else retVal = false;
            if (parameter is string sp && sp == "negative")
            {
                return !retVal;
            }
            else return retVal;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToNegative : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b) return !b;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
