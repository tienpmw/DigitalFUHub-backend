﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Statistic
{
	public class StatisticSalesResponseDTO
	{
		public int TypeStatistic { get; set; }
		public List<DataStatistic> DataStatistics { get; set; } = new();
	}
	public class DataStatistic
	{
		public int Date { get; set; }
		public long Revenue { get; set; }
		public long TotalOrders { get; set; }
	}
}
