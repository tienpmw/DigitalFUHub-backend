﻿using BusinessObject.Entities;
using DataAccess.DAOs;
using DataAccess.IRepositories;
using DTOs.Product;
using DTOs.Seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class ProductRepository : IProductRepository
	{
		public async Task AddProductAsync(Product product) => await ProductDAO.Instance.AddProductAsync(product);

		public List<SellerProductResponeDTO> GetAllProduct(int userId) => ProductDAO.Instance.GetAllProduct(userId);

		public List<ProductDetailVariantResponeDTO> GetProductVariants(int productId) => ProductDAO.Instance.GetProductVariants(productId);

        public ProductDetailResponseDTO GetProductById(long productId)
        {
            if (productId == 0) throw new ArgumentException("productId cannot eq 0 (at getProductById)");
            return ProductDAO.Instance.GetProductById(productId);
        }

    }
}
