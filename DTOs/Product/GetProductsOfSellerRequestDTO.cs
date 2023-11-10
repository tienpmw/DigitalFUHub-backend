﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Product
{
	public class GetProductsOfSellerRequestDTO
	{
		public long UserId { get; set; }
		public long ProductId { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public int ProductCategory { get; set; }
		public int ProductStatusId { get; set; }
		public int SoldMin { get; set; }
		public int SoldMax { get; set; }
		public int Page { get; set; }
	}
}
