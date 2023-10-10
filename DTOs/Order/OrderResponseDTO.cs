﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Order
{
	public class OrderResponseDTO
	{
		public int NextOffset { get; set; }
		public List<OrderProductResponseDTO> Orders { get; set; } = new();
	}

	public class OrderProductResponseDTO
	{
		public long OrderId { get; set; }
		public long ShopId { get; set; }
		public string ShopName { get; set; } = string.Empty;
		public long ProductId { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public string Thumbnail { get; set; } = string.Empty;
		public int Quantity { get; set; }
		public long Price { get; set; }
		public string ProductVariantName { get; set; } = string.Empty;
		public bool IsFeedback { get; set; }
		public long Discount { get; set; }
		public long StatusId { get; set; }
		public long CouponDiscount { get; set; }
		public List<string> Assest { get; set; } = null!;
	}
}