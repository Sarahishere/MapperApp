
using MapperApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MapperApp.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Driver> Drivers { get; set; }
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
            
        }
    }
}