using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunitureApp.Models
{
    public class UserOrder
    {
        public int Id { get; set; }
        public string  Order_no { get; set; }
        public int User_id { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public int Delivery_method_id { get; set; }
        public int Delivery_free { get; set; }
        public DateTime? Create_at { get; set; }


    }
}
