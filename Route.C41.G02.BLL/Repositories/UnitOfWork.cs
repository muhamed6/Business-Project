using Microsoft.EntityFrameworkCore;
using Route.C41.G02.BLL.Interfaces;
using Route.C41.G02.DAL.Data;
using Route.C41.G02.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Route.C41.G02.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork

    {

        private readonly ApplicationDbcontext _dbContext;

        private Hashtable _repositories; 



        public UnitOfWork(ApplicationDbcontext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }

        public async Task<int> Complete()
        {
            return await _dbContext.SaveChangesAsync();
        }

        // function dispose to close the db connection 
        public async ValueTask DisposeAsync() 
        {
           await _dbContext.DisposeAsync();
        }

        public IGenericRepository<T> Repository<T>() where T : ModelBase
        {
            var key=typeof(T).Name;

            if(!_repositories.ContainsKey(key))
            {

                if (key==nameof(Employee))
                {
                    var repository = new EmployeeRepository(_dbContext);
                    _repositories.Add(key, repository);
                }

                else
                {
                  var  repository = new GenericRepository<T>(_dbContext);
                    _repositories.Add(key, repository);
                }



            }

            return _repositories[key] as IGenericRepository<T>;


         }

         
    }
}
