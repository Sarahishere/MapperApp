
using MapperApp.Core;
using MapperApp.Core.Repositories;

namespace MapperApp.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApiDbContext _context;
        public IDriverRepository Drivers { get; }
        
        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
           

            Drivers = new DriverRepository(_context);
        }

        public void CompleteChange()
        {
           _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}