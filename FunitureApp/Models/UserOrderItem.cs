using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Models
{
    public class UserOrderItem
    {
        public int Id { get; set; }
        public int User_order_id { get; set; }
        public int Product_attr_id { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
