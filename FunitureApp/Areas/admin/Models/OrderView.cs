using System;
using System.Collections.Generic;
using FunitureApp.Models;

namespace FunitureApp.Areas.admin.Models
{
    public class OrderView
    {
        public UserOrder Order { get; set; }
        public User User { get; set; }
        public UserAddress Address { get; set; }

        public List<UserOrderItem> OrderItem { get; set; }
    }
}

