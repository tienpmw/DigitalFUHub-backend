﻿using BusinessObject.Entities;
using DataAccess.DAOs;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class CouponRepository : ICouponRepository
	{
		public void AddCoupon(Coupon coupon) => CouponDAO.Instance.AddCoupon(coupon);

		public bool IsExistCouponCode(long shopId, string couponCode,string action) => CouponDAO.Instance.IsExistCouponCode(shopId, couponCode, action);

        public Coupon? GetCouponPrivate(string couponCode, long shopId) => CouponDAO.Instance.GetCouponPrivate(couponCode, shopId);

        public List<Coupon> GetCouponPublic (long shopId) => CouponDAO.Instance.GetCouponPublic(shopId);

		public Coupon? GetCoupon(long couponId, long shopId) => CouponDAO.Instance.GetCoupon(couponId, shopId);

		public (long, List<Coupon>) GetListCouponsByShop(long userId, string couponCode, DateTime? startDate, 
			DateTime? endDate, bool? isPublic, long status, int page)
		=> CouponDAO.Instance.GetListCouponsByShop(userId, couponCode, startDate, endDate, isPublic, status, page);

		public void UpdateStatusCoupon(Coupon coupon) => CouponDAO.Instance.UpdateStatusCoupon(coupon);

		public void UpdateCoupon(Coupon coupon) => CouponDAO.Instance.UpdateCoupon(coupon);

		public void UpdateCouponFinish(long couponId, long userId) => CouponDAO.Instance.UpdateCouponFinish( couponId, userId);

        public Coupon? GetCouponDetailCustomer(long couponId) => CouponDAO.Instance.GetCouponDetailCustomer(couponId);

        public Coupon? GetCouponEntityById(long couponId) => CouponDAO.Instance.GetCouponEntityById(couponId);
    }
}
