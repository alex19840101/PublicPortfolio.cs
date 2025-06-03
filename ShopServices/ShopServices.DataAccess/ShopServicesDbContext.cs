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

        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<EmailNotification> EmailNotifications { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderPosition> OrderPositions { get; set; }
        public DbSet<PhoneNotification> PhoneNotifications { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeesConfiguration());
            modelBuilder.ApplyConfiguration(new BuyersConfiguration());

            modelBuilder.ApplyConfiguration(new AvailabilitiesConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriesConfiguration());
            modelBuilder.ApplyConfiguration(new CouriersConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveriesConfiguration());
            modelBuilder.ApplyConfiguration(new EmailNotificationsConfiguration());
            modelBuilder.ApplyConfiguration(new ManagersConfiguration());
            modelBuilder.ApplyConfiguration(new OrdersConfiguration());
            modelBuilder.ApplyConfiguration(new OrderPositionsConfiguration());
            modelBuilder.ApplyConfiguration(new PhoneNotificationsConfiguration());
            modelBuilder.ApplyConfiguration(new PricesConfiguration());
            modelBuilder.ApplyConfiguration(new ProductsConfiguration());
            modelBuilder.ApplyConfiguration(new ShopsConfiguration());
            modelBuilder.ApplyConfiguration(new TradesConfiguration());
            modelBuilder.ApplyConfiguration(new WarehousesConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
