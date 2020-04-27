using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace Martivi.Model
{
    public class ProductsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ProductTemplate { get; set; }
        public DataTemplate ErrorTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return (string)item == "Xamarin.Forms" ? ProductTemplate : ErrorTemplate;
        }
    }
    public class CategoryOrProductTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CategoryTemplate { get; set; }
        public DataTemplate ProductTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Category) return CategoryTemplate;
            if (item is Product) return ProductTemplate;
            throw new Exception("Unknown type");
        }
    }
}
