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
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly DemoMvcAppContext _dbContext;

		public GenericRepository(DemoMvcAppContext dbContext)
        {
			_dbContext = dbContext;
		}
        public async Task AddAsync(T item)
		{
			 await _dbContext.AddAsync(item);
		}

		public  void Delete(T item)
		{
			 _dbContext.Remove(item);
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			if(typeof(T) == typeof(Employee))
			{
				return  (IEnumerable<T>)await _dbContext.Employees.Include(E => E.Department).ToListAsync();	
			}

			return await _dbContext.Set<T>().ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbContext.Set<T>().FindAsync(id);

		}

		public void Update(T item)
		{
			_dbContext.Update(item);
		}
	}
}
