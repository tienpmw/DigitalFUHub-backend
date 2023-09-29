﻿using BusinessObject.Entities;
using DTOs.Seller;
using DTOs.Shop;
using DTOs.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Product
{
    public class ProductDetailResponseDTO
    {
        public long ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Thumbnail { get; set; }
        public string? Description { get; set; }
        public int Discount { get; set; }
        public long ShopId { get; set; }
        public long CategoryId { get; set; }
        public List<ProductDetailVariantResponeDTO>? ProductVariants { get; set; }
        public List<ProductMediaResponseDTO>? ProductMedias { get; set; }
        public List<TagResponseDTO>? Tags { get; set; }
    }




}