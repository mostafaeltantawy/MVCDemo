﻿using DEMO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
		IQueryable<Employee> GetEmployeeByAddress(string address);
        IQueryable<Employee> GetEmployeeByName(string SearchValue);
	
	}
}
