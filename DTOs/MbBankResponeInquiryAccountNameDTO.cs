﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
	public class MbBankResponeInquiryAccountNameDTO
	{
		public string? refNo { get; set; }
		public Result result { get; set; } = null!;
		public string? benName { get; set; }
		public string? category { get; set; }
	}

}
