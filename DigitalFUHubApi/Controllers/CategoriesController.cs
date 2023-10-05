﻿using BusinessObject.Entities;
using DataAccess.IRepositories;
using DigitalFUHubApi.Comons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalFUHubApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoriesController(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}
		[HttpGet("GetAll")]
		public ActionResult<ResponseData> GetAll()
		{
			List<Category> categories = _categoryRepository.GetAll();
			return new ResponseData
			{
				Status = new Status
				{
					Message = "",
					Ok = true,
					ResponseCode = Constants.RESPONSE_CODE_SUCCESS
				},
				Result = categories
			};
		}
	}
}
