﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace BusinessObject.Entities
{
    public class Product
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ProductId { get; set; }
        public long ShopId { get; set; }
        public long CategoryId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        public string? Description { get; set; }
        public int Discount { get; set; }
        public string? Thumbnail { get; set; }
		public DateTime DateCreate { get; set; }
		public DateTime DateUpdate { get; set; }
		public DateTime BanDate { get; set; }
		public long TotalRatingStar { get; set; }
        public long NumberFeedback { get; set; }
		public int ViewCount { get; set; }
		public int LikeCount { get; set; }
		public int SoldCount { get; set; }
        public string Note { get; set; } = string.Empty;
		public long ProductStatusId { get; set; }
		[ForeignKey(nameof(ShopId))]
        public virtual Shop Shop { get; set; } = null!;
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = null!;
        [ForeignKey(nameof(ProductStatusId))]
        public virtual ProductStatus ProductStatus { get; set; } = null!;
        public virtual ICollection<ProductVariant>? ProductVariants { get; set; }
        public virtual ICollection<ProductMedia> ProductMedias { get; set; } = null!;
        public virtual ICollection<Feedback>? Feedbacks { get; set; }
        public virtual ICollection<Tag>? Tags{ get; set; }
		public virtual ICollection<CouponProduct>? CouponProducts { get; set; }
		public virtual ICollection<WishList>? WishList { get; set; }
		public virtual ICollection<ReportProduct>? ReportProducts { get; set; }
	}
}
