﻿using AutoMapper;
using BusinessObject.Entities;
using DataAccess.IRepositories;
using DigitalFUHubApi.Comons;
using DigitalFUHubApi.Hubs;
using DigitalFUHubApi.Managers;
using DTOs.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Comons;
using DataAccess.DAOs;
using DataAccess.Repositories;

namespace DigitalFUHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {

        private readonly IConnectionManager _connectionManager;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly IAssetInformationRepository _assetInformationRepository;

        public CartsController(IConnectionManager connectionManager,
            ICartRepository cartRepository, IMapper mapper, IAssetInformationRepository assetInformationRepository)
        {
            _connectionManager = connectionManager;
            _cartRepository = cartRepository;
            _mapper = mapper;
            _assetInformationRepository = assetInformationRepository;
        }


        [HttpPost("addProductToCart")]
        [Authorize]
        public IActionResult AddProductToCart([FromBody] CartDTO addProductToCartRequest)
        {
            try
            {
                if (addProductToCartRequest == null || addProductToCartRequest.UserId == 0 ||
                    addProductToCartRequest.ProductVariantId == 0 || addProductToCartRequest.Quantity == 0)
                {
                    return BadRequest(new Status());
                }
                var resultCheck = _cartRepository.CheckQuantityForCart(addProductToCartRequest.UserId,
                                                                           addProductToCartRequest.ProductVariantId,
                                                                           addProductToCartRequest.Quantity);
                bool resultBool = resultCheck.Item1;
                long cartQuantity = resultCheck.Item2;
                if (!resultBool)
                {
                    return Ok(new Status
                    {
                        ResponseCode = Constants.CART_RESPONSE_CODE_INVALID_QUANTITY,
                        Message = $"Sản phẩm này đang có số lượng {cartQuantity} trong giỏ hàng của bạn," +
                            $" không thể thêm số lượng đã chọn vào giỏ hàng vì đã vượt quá số lượng sản phẩm có sẵn",
                        Ok = resultBool
                    });
                }
                _cartRepository.AddProductToCart(addProductToCartRequest);

                return Ok(new Status
                {
                    ResponseCode = Constants.CART_RESPONSE_CODE_SUCCESS,
                    Message = "",
                    Ok = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new Status());
            }
        }


        [HttpGet("GetCartsByUserId/{userId}")]
        //[Authorize]
        public async Task<IActionResult> GetCartsByUserId(long userId)
        {
            try
            {
                return Ok(await _cartRepository.GetCartsByUserId(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPut("Update")]
        [Authorize]
        public IActionResult UpdateCart(UpdateCartRequestDTO? updateCartRequest)
        {
            try
            {
                if (updateCartRequest == null || updateCartRequest.UserId == 0 ||
                        updateCartRequest.ProductVariantId == 0)
                {
                    return BadRequest(new Status());
                }
                long quantityProductVariant = _assetInformationRepository.GetByProductVariantId(updateCartRequest.ProductVariantId).Count();
                if (updateCartRequest.Quantity == 0)
                {
                    var cart = _cartRepository.GetCart(updateCartRequest.UserId, updateCartRequest.ProductVariantId);
                    if (cart != null)
                    {
                        if (cart.Quantity > quantityProductVariant)
                        {
                            updateCartRequest.Quantity = quantityProductVariant;
                            _cartRepository.UpdateCart(_mapper.Map<Cart>(updateCartRequest));
                            return Ok(new Status
                            {
                                ResponseCode = Constants.CART_RESPONSE_CODE_CART_PRODUCT_INVALID_QUANTITY,
                                Message = $"Rất tiếc, bạn chỉ có thể mua số lượng tối đa {quantityProductVariant} sản phẩm " +
                                $"(Số lượng sản phẩm trong giỏ hàng của bạn đã được thay đổi thành {quantityProductVariant})",
                                Ok = false
                            });
                        } else {
                            return Ok(new Status
                            {
                                ResponseCode = Constants.CART_RESPONSE_CODE_SUCCESS,
                                Message = "",
                                Ok = true
                            });
                        }
                    }
                    else
                    {
                        return NotFound(new Status());
                    }
                }

                var resultCheck = !(updateCartRequest.Quantity > quantityProductVariant);
                if (!resultCheck)
                {
                    updateCartRequest.Quantity = quantityProductVariant;
                    _cartRepository.UpdateCart(_mapper.Map<Cart>(updateCartRequest));
                    return Ok(new Status
                    {
                        ResponseCode = Constants.CART_RESPONSE_CODE_INVALID_QUANTITY,
                        Message = $"Sản phẩm này đang có số lượng tối đa là {quantityProductVariant} " +
                        $"(số lượng sản phẩm trong giỏ hàng của bạn đã được thay đổi thành {quantityProductVariant})",
                        Ok = resultCheck
                    });
                }

                _cartRepository.UpdateCart(_mapper.Map<Cart>(updateCartRequest));
                return Ok(new Status
                {
                    ResponseCode = Constants.CART_RESPONSE_CODE_SUCCESS,
                    Message = "",
                    Ok = true
                });


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new Status());
            }
        }

        [HttpPost("DeleteCart")]
        [Authorize]
        public async Task<IActionResult> DeleteCart([FromBody] DeleteCartRequestDTO deleteCartRequest)
        {
            try
            {
                await _cartRepository.DeleteCart(deleteCartRequest.UserId, deleteCartRequest.ProductVariantId);
                return Ok(new Status
                {
                    Message = "Delete Cart Successfully",
                    ResponseCode = Constants.RESPONSE_CODE_SUCCESS,
                    Ok = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Status
                {
                    Message = ex.Message
                    ,
                    ResponseCode = Constants.RESPONSE_CODE_FAILD
                    ,
                    Ok = false
                });
            }
        }
    }
}