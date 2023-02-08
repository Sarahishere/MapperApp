
using MapperApp.Models;

namespace MapperApp.Core
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        Driver? GetByDriverNum(int driverNum);
    }
}