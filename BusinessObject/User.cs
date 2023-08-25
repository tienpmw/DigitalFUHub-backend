﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class User
	{
		

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long UserId { get; set; }
		public long RoleId { get; set; }
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public bool Status { get; set; }	

		[ForeignKey(nameof(RoleId))]
		public virtual Role Role { get; set; } = null!;
		public virtual ICollection<AccessToken>? AccessTokens { get; set; }
	}
}