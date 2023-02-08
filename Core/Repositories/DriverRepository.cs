

using MapperApp.Data;
using MapperApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MapperApp.Core.Repositories
{
    public class DriverRepository : GenericRepository<Driver>, IDriverRepository
    {
        public DriverRepository(ApiDbContext context) : base(context)
        {
        }

        public override  IEnumerable<Driver> All()
        {
            try
            {
                return _context.Drivers.Where(x => x.Status == 1).ToList();
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public Driver? GetByDriverNum(int driverNum)
        {
           try
            {
                return  _context.Drivers.FirstOrDefault(x => x.DriverNumber == driverNum);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}