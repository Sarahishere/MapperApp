
using MapperApp.Data;
using Microsoft.EntityFrameworkCore;

namespace MapperApp.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApiDbContext _context;
        internal DbSet<T> _dbSet;
       

        public GenericRepository(
            ApiDbContext context
        )
        {
            _context = context;
            this._dbSet = _context.Set<T>();
        }
        public  bool Add(T entity)
        {
            _dbSet.Add(entity);
            return true;
        }

        public virtual  IEnumerable<T> All()
        {
            return _dbSet.ToList();
        }

        public bool Delete(T entity)
        {
             _dbSet.Remove(entity);
            return true;
        }

        public  T? GetById(Guid id)
        {
            return  _dbSet.Find(id);
        }

        public virtual bool Update(T entity)
        {
             _dbSet.Update(entity);
             return true;
        }
    }
}