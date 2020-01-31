using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Martivi.Model
{
    public enum OrderStatus
    {
        Accepted,
        Canceled,
        Completed
    }
    public class Order:PropertyChangedBase
    {
        private OrderStatus _Status;

        public OrderStatus Status
        {
            get { return _Status; }
            set { _Status = value; OnPropertyChanged(); }
        }


        public int OrderId { get; set; }
        public User User { get; set; }
        public virtual List<Product> OrderedProducts { get; set; }

        public long OrderTimeTicks { get; set; }

        [JsonIgnore]
        public DateTime OrderTime
        {
            get 
            {
                DateTime t = new DateTime(OrderTimeTicks, DateTimeKind.Local);
                return t;
            }
            set { OrderTimeTicks = value.Ticks; }
        }


    }
}
