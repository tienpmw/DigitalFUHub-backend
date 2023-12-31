﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Entities
{
	public class Tag
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long TagId { get; set; }
		public long ProductId { get; set; }
		public string? TagName { get; set; }

		[ForeignKey(nameof(ProductId))]
		public virtual Product Product { get; set; } = null!;	
	}
}
