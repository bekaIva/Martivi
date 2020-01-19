using System;
using System.Collections.Generic;
using System.Text;

namespace Martivi.Model
{
    public class Order
    {

        public int OrderId { get; set; }
        public User User { get; set; }
        public virtual List<Product> Products { get; set; }

    }
}
