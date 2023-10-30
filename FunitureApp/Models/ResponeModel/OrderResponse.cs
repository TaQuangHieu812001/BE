using System;
using System.Collections.Generic;

namespace FunitureApp.Models.ResponeModel
{
	public class OrderResponse
	{
		public UserOrder order { get; set; }
		public List<OrderItemResponse> orderItems { get; set; }
		
	}
	public class OrderItemResponse
	{
		public UserOrderItem orderItem { get; set; }
		public ProductAttribute productAttribute { get; set; }
		public Product product { get; set; }
	}
}

