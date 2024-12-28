using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO.BLL.Interfaces
{
    public interface IUnitOfWork  
    {
        //Signature For Property OfEach And Every Repository interface
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        Task<int> CompleteAsync();
        


    }
}
