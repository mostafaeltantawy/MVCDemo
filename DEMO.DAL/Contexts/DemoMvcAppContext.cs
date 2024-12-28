using DEMO.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEMO.DAL.Contexts
{
    public class DemoMvcAppContext : IdentityDbContext<ApplicationUser>
    {

        public DemoMvcAppContext(DbContextOptions<DemoMvcAppContext> options):base(options)
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlServer("Server = . ; DataBase = DemoMvcApp ; Trusted_Connection = True ;  ");
        //    base.OnConfiguring(optionsBuilder);
        //}

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
