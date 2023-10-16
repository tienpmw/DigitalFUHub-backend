﻿using BusinessObject.Entities;
using DTOs.Cart;
using DTOs.Seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface ICartRepository
    {
        void AddProductToCart(AddProductToCartRequestDTO addProductToCartRequest);

        Cart? GetCart(long userId, long productVariantId);

        List<CartGroupResponseDTO> GetCartsByUserId(long userId);

        Task DeleteCart(long userId, long productVariantId);

        (bool, long) CheckQuantityForCart(long userId, long productVariantId, long quantity);

        void UpdateCart(Cart newCart);
    }
}
