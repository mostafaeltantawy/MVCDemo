using DEMO.BLL.Interfaces;
using DEMO.DAL.Contexts;
using DEMO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department> ,  IDepartmentRepository
    {
        public DepartmentRepository(DemoMvcAppContext dbSContext) : base(dbSContext)
		{
            
        }

    }
}
