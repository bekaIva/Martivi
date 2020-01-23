
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MartiviSharedLib.Models
{
    public enum OrderStatus
    {
        Accepted,
        Completed
    }
    public class OrderBase
    {
        public OrderStatus Status { get; set; }
        public int OrderId { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        
    }
}