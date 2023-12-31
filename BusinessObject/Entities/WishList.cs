﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
	public class WishList
	{
		public long UserId { get; set; }	
		public long ProductId { get; set;}

		[ForeignKey(nameof(UserId))]
		public virtual User User { get; set; } = null!;
		[ForeignKey(nameof(ProductId))]
		public virtual Product Product { get; set; } = null!;
	}
}
