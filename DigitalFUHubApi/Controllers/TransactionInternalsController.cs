﻿using AutoMapper;
using Azure.Core;
using BusinessObject.Entities;
using Comons;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using DigitalFUHubApi.Comons;
using DigitalFUHubApi.Services;
using DTOs.Bank;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalFUHubApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TransactionInternalsController : ControllerBase
	{
		private readonly ITransactionInternalRepository transactionRepository;
		private readonly JwtTokenService JwtTokenService;
		private readonly IMapper mapper;

		public TransactionInternalsController(ITransactionInternalRepository transactionRepository, JwtTokenService jwtTokenService, IMapper mapper)
		{
			this.transactionRepository = transactionRepository;
			JwtTokenService = jwtTokenService;
			this.mapper = mapper;
		}



		#region Get History transaction of internal for admin
		[Authorize(Roles = "Admin")]
		[HttpPost("All")]
		public IActionResult GetHistoryTransactionInternal(HistoryTransactionInternalRequestDTO request)
		{
			if (!ModelState.IsValid) return BadRequest();

			const int TRANSACTION_TYPE_ALL = 0;
			int[] transactionTypes = Constants.TRANSACTION_INTERNAL_STATUS_TYPE;
			if (!transactionTypes.Contains(request.TransactionInternalTypeId) && request.TransactionInternalTypeId != TRANSACTION_TYPE_ALL)
			{
				return Ok(new ResponseData(Constants.RESPONSE_CODE_NOT_ACCEPT, "Invalid transaction type!", false, new()));
			}

			try
			{
				(bool isValid, DateTime? fromDate, DateTime? toDate) = Util.GetFromDateToDate(request.FromDate, request.ToDate);
				if (!isValid)
				{
					return Ok(new ResponseData(Constants.RESPONSE_CODE_NOT_ACCEPT, "Invalid date", false, new()));
				}

				long orderId;
				long.TryParse(request.OrderId, out orderId);

				var totalRecord = transactionRepository.GetNumberTransactionInternal(orderId, request.Email, fromDate, toDate, request.TransactionInternalTypeId);
				var numberPages = totalRecord / Constants.PAGE_SIZE + 1;
				if (request.Page > numberPages || request.Page == 0)
				{
					return Ok(new ResponseData(Constants.RESPONSE_CODE_NOT_ACCEPT, "Invalid number page", false, new()));
				}

				var transactions = transactionRepository.GetHistoryTransactionInternal(orderId, request.Email, fromDate, toDate, request.TransactionInternalTypeId, request.Page);

				var result = new 
				{
					Total = totalRecord,
					TransactionInternals = mapper.Map<List<HistoryTransactionInternalResponseDTO>>(transactions)
				};

				return Ok(new ResponseData(Constants.RESPONSE_CODE_SUCCESS, "Success", true, result));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		#endregion

		#region Get History transaction of internal for user 
		[Authorize]
		[HttpPost("data")]
		public IActionResult GetHistoryTransactionInternalUser(HistoryTransactionUserRequestDTO request)
		{
			if (!ModelState.IsValid) return BadRequest();

			var userId = JwtTokenService.GetUserIdByAccessToken(User);

			const int TRANSACTION_TYPE_ALL = 0;
			int[] transactionTypes = Constants.TRANSACTION_INTERNAL_STATUS_TYPE;
			if (!transactionTypes.Contains(request.TransactionInternalTypeId) && request.TransactionInternalTypeId != TRANSACTION_TYPE_ALL)
			{
				return Ok(new ResponseData(Constants.RESPONSE_CODE_NOT_ACCEPT, "Invalid transaction type!", false, new()));
			}

			try
			{
				(bool isValid, DateTime? fromDate, DateTime? toDate) = Util.GetFromDateToDate(request.FromDate, request.ToDate);
				if (!isValid)
				{
					return Ok(new ResponseData(Constants.RESPONSE_CODE_NOT_ACCEPT, "Invalid date", false, new()));
				}

				long orderId;
				long.TryParse(request.OrderId, out orderId);

				var totalRecord = transactionRepository.GetNumberTransactionInternalOfUser(userId, orderId, fromDate, toDate, request.TransactionInternalTypeId);
				var numberPages = totalRecord / Constants.PAGE_SIZE + 1;
				if (request.Page > numberPages || request.Page == 0)
				{
					return Ok(new ResponseData(Constants.RESPONSE_CODE_NOT_ACCEPT, "Invalid number page", false, new()));
				}

				var transactions = transactionRepository.GetHistoryTransactionInternalOfUser(userId, orderId, fromDate, toDate, request.TransactionInternalTypeId, request.Page);

				var result = new
				{
					Total = totalRecord,
					TransactionInternals = mapper.Map<List<HistoryTransactionInternalResponseDTO>>(transactions)
				};

				return Ok(new ResponseData(Constants.RESPONSE_CODE_SUCCESS, "Success", true, result));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		#endregion

		#region Get data report History transaction of internal 
		[Authorize(Roles = "Admin")]
		[HttpPost("GetDataReportTransactionInternal")]
		public IActionResult GetDataReportTransactionInternal(HistoryTransactionInternalRequestDTO request)
		{
			if (!ModelState.IsValid) return BadRequest();

			const int TRANSACTION_TYPE_ALL = 0;
			int[] transactionTypes = Constants.TRANSACTION_INTERNAL_STATUS_TYPE;
			if (!transactionTypes.Contains(request.TransactionInternalTypeId) && request.TransactionInternalTypeId != TRANSACTION_TYPE_ALL)
			{
				return Ok(new ResponseData(Constants.RESPONSE_CODE_NOT_ACCEPT, "Invalid transaction type!", false, new()));
			}

			try
			{
				(bool isValid, DateTime? fromDate, DateTime? toDate) = Util.GetFromDateToDate(request.FromDate, request.ToDate);
				if (!isValid)
				{
					return Ok(new ResponseData(Constants.RESPONSE_CODE_NOT_ACCEPT, "Invalid date", false, new()));
				}

				long orderId;
				long.TryParse(request.OrderId, out orderId);

				var transactionInternals = transactionRepository.GetDataReportTransactionInternal(orderId, request.Email, fromDate, toDate, request.TransactionInternalTypeId);

				var result = mapper.Map<List<HistoryTransactionInternalResponseDTO>>(transactionInternals);

				return Ok(new ResponseData(Constants.RESPONSE_CODE_SUCCESS, "Success", true, result));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
		#endregion

	}
}
