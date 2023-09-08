﻿using BusinessObject;
using DataAccess.DAOs;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class StorageRepository : IStorageRepository
	{
		public void AddFile(Storage file) => StorageDAO.Instance.AddFile(file);

		public Storage? GetFileByName(string filename) => StorageDAO.Instance.GetFileByName(filename);

		public async Task<Stream?> GetFileFromAzureAsync(string filename) => await StorageDAO.Instance.GetFileFromAzureAsync(filename);
	}
}