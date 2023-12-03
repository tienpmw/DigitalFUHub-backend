﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Slider
{
    public class SliderAdminAddRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        public long ProductId { get; set; }
        public bool IsActive { get; set; }
    }
}
