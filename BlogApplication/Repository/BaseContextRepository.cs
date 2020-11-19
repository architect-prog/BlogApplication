using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApplication.Repository
{
    public abstract class BaseContextRepository<TContext, TEntity> : IRepository<TEntity> 
        where TEntity: class
        where TContext: DbContext
    {
        protected TContext _context;

        public BaseContextRepository(TContext context)
        {
            _context = context;
        }

        public abstract Task<TEntity> Add(TEntity entity);
        public abstract Task<TEntity> Delete(int id);
        public abstract List<TEntity> GetAll();
        public abstract Task<TEntity> GetById(int id);
        public abstract Task<TEntity> Update(TEntity entity);
        
    }
}
