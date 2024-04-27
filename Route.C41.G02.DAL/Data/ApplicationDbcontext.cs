using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Route.C41.G02.DAL.Data.Configurations;
using Route.C41.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G02.DAL.Data
{
    public class ApplicationDbcontext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) :base(options)
        {

        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder); 


            modelBuilder.Entity<IdentityRole>().ToTable("Roles"); 





            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        


        }

        public DbSet<Department> Departments {  get; set; } 
        public DbSet<Employee> Employees {  get; set; } 


    }
}
