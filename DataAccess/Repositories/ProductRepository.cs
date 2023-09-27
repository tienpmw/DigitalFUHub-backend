﻿using DataAccess.DAOs;
using DataAccess.IRepositories;
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
		public List<ProductResponeDTO> GetAllProduct(int userId) => ProductDAO.Instance.GetAllProduct(userId);

		public List<ProductVariantResponeDTO> GetProductVariants(int productId) => ProductDAO.Instance.GetProductVariants(productId);

	}
}