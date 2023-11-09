﻿using AutoMapper;
using BusinessObject.Entities;
using Comons;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using DigitalFUHubApi.Comons;
using DigitalFUHubApi.Services;
using DTOs.Feedback;
using DTOs.Seller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DigitalFUHubApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FeedbacksController : ControllerBase
	{
		private readonly IOrderRepository _orderRepository;
		private readonly IFeedbackRepository _feedbackRepository;
		private readonly StorageService _storageService;
		private readonly JwtTokenService _jwtTokenService;
		private readonly IMapper _mapper;

		public FeedbacksController(IOrderRepository orderRepository, IFeedbackRepository feedbackRepository, StorageService storageService, JwtTokenService jwtTokenService, IMapper mapper)
		{
			_orderRepository = orderRepository;
			_feedbackRepository = feedbackRepository;
			_storageService = storageService;
			_jwtTokenService = jwtTokenService;
			_mapper = mapper;
		}


		#region Get all feedback
		[HttpGet("GetAll")]
		public IActionResult GetAll(long productId)
		{
			try
			{
				return Ok(_feedbackRepository.GetFeedbacks(productId));
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
		}
		#endregion

		#region add feedback order
		[Authorize("Customer,Seller")]
		[HttpPost("Customer/Add")]
		public async Task<IActionResult> AddFeedbackOrder([FromForm] CustomerFeedbackOrderRequestDTO request)
		{
			
			try
			{
				if (request.UserId != _jwtTokenService.GetUserIdByAccessToken(User))
				{
					return Unauthorized();
				}
				if (!ModelState.IsValid)
				{
					return Ok(new ResponseData(Constants.RESPONSE_CODE_NOT_ACCEPT, "INVALID", false, new()));
				}

				var orderDetail = _orderRepository.GetOrderDetail(request.OrderDetailId);
				if (orderDetail == null)
				{
					return Ok(new ResponseData(Constants.RESPONSE_CODE_DATA_NOT_FOUND, "Not found", false, new()));
				}

				if (orderDetail.Order.OrderStatusId != Constants.ORDER_STATUS_CONFIRMED)
				{
					return Ok(new ResponseData(Constants.RESPONSE_CODE_FEEDBACK_ORDER_UN_COMFIRM, "Order's status confirm not yet!", false, new()));
				}

				string[] fileExtension = new string[] { ".jpge", ".png", ".jpg" };
				List<string> urlImages = new List<string>();
				if (request.ImageFiles != null
					&&
					request?.ImageFiles?.Count > 0
					&&
					!request.ImageFiles.Any(x => !fileExtension.Contains(x.FileName.Substring(x.FileName.LastIndexOf(".")))))
				{
					string filename;
					DateTime now;
					foreach (IFormFile file in request.ImageFiles)
					{
						now = DateTime.Now;
						filename = string.Format("{0}_{1}{2}{3}{4}{5}{6}{7}{8}", request.UserId, now.Year, now.Month, now.Day, now.Millisecond, now.Second, now.Minute, now.Hour, file.FileName.Substring(file.FileName.LastIndexOf(".")));
						string path = await _storageService.UploadFileToAzureAsync(file, filename);
						urlImages.Add(path);
					}
				}


				_feedbackRepository.AddFeedbackOrder(request.UserId, request.OrderId, request.OrderDetailId, request.Content, request.Rate, urlImages);
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
			return Ok(new ResponseData(Constants.RESPONSE_CODE_SUCCESS, "SUCCESS", true, new()));
		}
		#endregion

		#region get feedback detail
		[HttpGet("Customer/{userId}/{orderId}")]
		public IActionResult GetFeedbackDetailOrder(long userId, long orderId)
		{
			try
			{
				Order? order = _feedbackRepository.GetFeedbackDetail(orderId, userId);
				if (order == null)
				{
					return Ok(new ResponseData(Constants.RESPONSE_CODE_DATA_NOT_FOUND, "NOT FOUND", false, new()));
				}
				List<CustomerFeedbackDetailOrderResponseDTO> response = order.OrderDetails.Where(x => x.IsFeedback == true).Select(x => new CustomerFeedbackDetailOrderResponseDTO
				{
					Username = order.User.Username,
					Avatar = order.User.Avatar,
					ProductName = x.ProductVariant.Product.ProductName ?? "",
					ProductVariantName = x.ProductVariant.Name ?? "",
					Content = x?.Feedback?.Content ?? "",
					Rate = x.Feedback?.Rate ?? 0,
					Quantity = x.Quantity,
					Date = x.Feedback?.UpdateDate ?? new DateTime(),
					Thumbnail = x.ProductVariant.Product.Thumbnail ?? "",
					UrlImages = x.Feedback == null || x.Feedback?.FeedbackMedias == null ? new List<string>() : x.Feedback.FeedbackMedias.Select(x => x.Url).ToList(),
				}).ToList();
				return Ok(new ResponseData(Constants.RESPONSE_CODE_SUCCESS, "SUCCESS", true, response));
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}

		}
		#endregion

		#region get list feedback of seller
		[HttpPost("Seller/List")]
		public IActionResult GetListFeedbackSeller(SellerFeedbackRequestDTO request)
		{
			try
			{
				int[] rates = new[] { 0, 1, 2, 3, 4, 5 };
				if (request.UserId != _jwtTokenService.GetUserIdByAccessToken(User))
				{
					return Unauthorized();
				}
				if (!ModelState.IsValid || !rates.Contains(request.Rate))
				{
					return Ok(new ResponseData(Constants.RESPONSE_CODE_NOT_ACCEPT, "INVALID", false, new()));
				}

				DateTime? fromDate = string.IsNullOrWhiteSpace(request.FromDate) ? null : DateTime.ParseExact(request.FromDate, "M/d/yyyy", CultureInfo.InvariantCulture);
				(long totalItems, List<Order> orders) = _feedbackRepository.GetListFeedbackSeller(request.UserId, request.OrderId, request.UserName.Trim(), request.ProductName.Trim(), request.ProductVariantName.Trim(), fromDate, request.Rate, request.Page);
				return Ok(new ResponseData(Constants.RESPONSE_CODE_SUCCESS, "SUCCESS", true, new ListFeedbackResponseDTO
				{
					TotalItems = totalItems,
					Feedbacks = _mapper.Map<List<SellerFeedbackResponseDTO>>(orders)
				}));
			}
			catch (Exception e)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
			}
		}
		#endregion
	}
}
