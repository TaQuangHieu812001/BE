using System;
using FunitureApp.Models;

namespace FunitureApp.Areas.admin.Models
{
	public class OrderView
	{
        public UserOrder Order { get; set; }
        public User User { get; set; }
        
    }
}

