using DEMO.BLL.Interfaces;
using DEMO.DAL.Contexts;
using DEMO.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO.BLL.Repositories
{
	public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
	{
		private readonly DemoMvcAppContext _dbSContext;

		public EmployeeRepository(DemoMvcAppContext dbSContext):base(dbSContext)
        {
			_dbSContext = dbSContext;
		}
        public IQueryable<Employee> GetEmployeeByAddress(string address)
		{
			return _dbSContext.Employees.Where(E => E.Address == address);
		}

        public IQueryable<Employee> GetEmployeeByName(string SearchValue)
        {
            return _dbSContext.Employees.Include(E => E.Department).Where(E => E.Name.ToLower().Contains(SearchValue));
        }
    }
}
