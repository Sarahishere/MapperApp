

namespace MapperApp.Core
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> All();
        T? GetById(Guid id);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);
    }
}