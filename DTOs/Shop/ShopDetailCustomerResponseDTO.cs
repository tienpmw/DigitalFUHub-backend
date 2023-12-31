﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Shop
{
    public class ShopDetailCustomerResponseDTO
    {
        public long UserId { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string ShopName { get; set; } = null!;
        public string? Description { get; set; }
        public long TotalRatingStar { get; set; }
        public long NumberFeedback { get; set; }
        public long ProductNumber { get; set; }
        public DateTime DateCreate { get; set; }
        public bool IsActive { get; set; }
        public ShopDetailCustomerUserResponseDTO? User { get; set; }

    }

    public class ShopDetailCustomerUserResponseDTO
    {
        public string Username { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public DateTime LastTimeOnline { get; set; }
    }
}
