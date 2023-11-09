﻿using BusinessObject.Entities;
using DTOs.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
	public interface IShopRepository
	{
		bool IsExistShopName(string shopName);
		void AddShop(string avatarUrl,string shopName, long userId, string shopDescription);
		bool IsExistShop(long userId);
		Shop? GetShopById(long shopId);
		void EditShop(Shop shop);
	}
}
