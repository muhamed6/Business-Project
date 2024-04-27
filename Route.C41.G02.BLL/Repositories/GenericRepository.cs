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
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbcontext _dbcontext; 

        public GenericRepository(ApplicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public  void Add(T entity)
        
        =>_dbcontext.Set<T>().Add(entity);
         

        public void Delete(T entity)
        
            =>_dbcontext.Set<T>().Remove(entity);
           

        public async Task <T> GetAsync(int id)
        {
           
            return await _dbcontext.FindAsync<T>(id); 
        }

        public  virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T)==typeof(Employee))
                return (IEnumerable<T>) await _dbcontext.Employees.Include(E=>E.Department).AsNoTracking().ToListAsync();

            else
                return await _dbcontext.Set<T>().AsNoTracking().ToListAsync();
        }


        public void Update(T entity)
        
           => _dbcontext.Set<T>().Update(entity);
          
    }
}
