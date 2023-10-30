﻿using BusinessObject;
using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
	internal class TransactionInternalDAO
	{
		private static TransactionInternalDAO? instance;
		private static readonly object instanceLock = new object();

		public static TransactionInternalDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new TransactionInternalDAO();
					}
				}
				return instance;
			}
		}

		internal void AddTransactionsForOrderConfirmed(Order order)
		{
			throw new NotImplementedException();
		}

		internal List<TransactionInternal> GetHistoryTransactionInternal(long orderId, string email, DateTime fromDate, DateTime toDate, int transactionTypeId)
		{
			List<TransactionInternal> transactions = new List<TransactionInternal>();
			using (DatabaseContext context = new DatabaseContext())
			{
				transactions = context.TransactionInternal
								.Include(x => x.User)
								.Where(x =>
									fromDate <= x.DateCreate && toDate >= x.DateCreate &&
									x.User.Email.Contains(email) &&
									(orderId == 0 ? true : x.OrderId == orderId) &&
									(transactionTypeId == 0 ? true : x.TransactionInternalTypeId == transactionTypeId)
									).OrderByDescending(x => x.DateCreate).ToList();
			}
			return transactions;
		}
	}
}