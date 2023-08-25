﻿using Microsoft.EntityFrameworkCore;
using BusinessObject;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using AutoMapper;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using ServerAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using ServerAPI.Middlewares;
using ServerAPI.Validations;

namespace ServerAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// Add database
			builder.Services.AddDbContext<ApiContext>(opts =>
			{
				opts.UseSqlServer(builder.Configuration.GetConnectionString("DB") ?? string.Empty);
			});

			//Add Odata
			builder.Services.AddControllers()
				.AddOData(options =>
				{
					options.Select().Filter().Count().OrderBy().SetMaxTop(100).Expand()
					.AddRouteComponents("odata", GetEdmModel());
				});

			// Add HttpContextAccessor 
			builder.Services.AddHttpContextAccessor();

			//Add JWT
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opt =>
				{
					opt.TokenValidationParameters = new JwtValidationParameters();
					opt.Events = new JwtValidationEvents();
				});

			//Add for more Policy Authorization
			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy("AdminOnly", policy =>
					policy.RequireRole("Admin"));
			});

			//Add Cors
			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(policy =>
				{
					policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
				});
			});


			// Disable auto model state validate
			builder.Services.Configure<ApiBehaviorOptions>(opts =>
			{
				opts.SuppressModelStateInvalidFilter = true;
			});

			//Add Auto Mapper
			var configAutoMapper = new MapperConfiguration(config =>
			{
				config.AddProfile(new AutoMapperProfile());
			});
			var mapper = configAutoMapper.CreateMapper();
			builder.Services.AddSingleton(mapper);

			// Remove cycle object's data in json respone
			builder.Services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			});

			//Add DI 
			builder.Services.AddSingleton<IUserRepository, UserRepository>();
			builder.Services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();
			builder.Services.AddSingleton<IAccessTokenRepository, AccessTokenRepository>();
			builder.Services.AddSingleton<IRoleRepository, RoleRepository>();

			builder.Services.AddSingleton<JwtTokenService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			// Add https
			//app.UseHttpsRedirection();

			app.UseCors();

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}

		private static IEdmModel GetEdmModel()
		{
			ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

			return builder.GetEdmModel();
		}


	}
}