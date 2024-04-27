using Microsoft.EntityFrameworkCore;
using Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.DAL.Data;
using Route.C41.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G02.BLL.Repositories
{
    public class EmployeeRepository:GenericRepository<Employee>, IEmployeeRepository
    {


        public EmployeeRepository(ApplicationDbcontext dbcontext) : base(dbcontext)
        {
       
        }

        public IQueryable<Employee> GetEmployeeByAddress(string address)
        {
           return _dbcontext.Employees.Where(E=>E.Address.ToLower() == address.ToLower());   
        }

       public IQueryable<Employee> SearchEmployeeByName(string name)
        =>_dbcontext.Employees.Where(E=>E.Name.ToLower().Contains(name)).Include(E=>E.Department).AsNoTracking();


        public override async Task <IEnumerable <Employee>> GetAllAsync()
            => await _dbcontext.Set<Employee>().Include(E=>E.Department).ToListAsync();


        public Employee Get(int id, bool asNoTracking = false)
        {
            if (asNoTracking)
            {
                return _dbcontext.Employees.AsNoTracking().FirstOrDefault(e => e.Id == id);
            }
            else
            {
                return _dbcontext.Employees.FirstOrDefault(e => e.Id == id);
            }
        }




    }
}
