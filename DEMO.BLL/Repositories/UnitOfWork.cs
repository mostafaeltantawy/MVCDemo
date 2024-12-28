using DEMO.BLL.Interfaces;
using DEMO.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
        private readonly DemoMvcAppContext _dbContext;

        public IEmployeeRepository EmployeeRepository { get ; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }


        public UnitOfWork(DemoMvcAppContext dbContext) // Ask CLR For Object Of DbContext
        {
            EmployeeRepository = new EmployeeRepository(dbContext);
            DepartmentRepository = new DepartmentRepository(dbContext);
            _dbContext = dbContext;
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();    
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
