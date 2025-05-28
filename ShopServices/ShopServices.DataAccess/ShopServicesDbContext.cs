using Microsoft.EntityFrameworkCore;
using ShopServices.DataAccess.Configurations;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess
{
    public class ShopServicesDbContext : DbContext
    {
        public ShopServicesDbContext(DbContextOptions<ShopServicesDbContext> options) : base(options)
        {
        
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        //public DbSet<_> _s { get; set; }
        //public DbSet<_> _s { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeesConfiguration());
            //modelBuilder.ApplyConfiguration(new _sConfiguration());
            //modelBuilder.ApplyConfiguration(new _sConfiguration());
            //modelBuilder.ApplyConfiguration(new _sConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
