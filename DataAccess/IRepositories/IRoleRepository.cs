﻿using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IRoleRepository
	{
		List<Role> GetAllRole();
		Role GetRole(long id);
	}
}
