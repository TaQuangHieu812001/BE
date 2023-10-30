using System;
using System.Collections.Generic;

namespace FunitureApp.Models.RequestModel
{
	public class OrderRequest
	{
		/// <summary>
		/// shippment Id
		/// </summary>
		public int shipId { get; set; }
		/// <summary>
		/// 0:atm, 1:cash
		/// </summary>
		public int paymentType { get; set; }
		public decimal deliveryFee { get; set; }
		public decimal total { get; set; }
		public List<OrderItemRequest> orderItem { get; set; }
	}
	public class OrderItemRequest
	{
		public Product product { get; set; }
		public ProductAttribute productAttribute { get; set; }
		public int count { get; set; }
	}
}

