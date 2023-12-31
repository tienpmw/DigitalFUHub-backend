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
	public class RoleRepository : IRoleRepository
	{
		public List<Role> GetAllRole() => RoleDAO.Instance.GetAllRole();

		public Role GetRole(long id) => RoleDAO.Instance.GetAllRole(id);
	}
}
