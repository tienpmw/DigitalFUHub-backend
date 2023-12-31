﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Cart
{
    public class AddProductToCartRequestDTO
    {
		public long UserId { get; set; } = 0!;
		public long ShopId { get; set; } = 0!;
		public long ProductVariantId { get; set; } = 0!;
		public int Quantity { get; set; } = 0!;
	}
}
