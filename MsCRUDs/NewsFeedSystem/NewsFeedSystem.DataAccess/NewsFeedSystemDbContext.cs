using Microsoft.EntityFrameworkCore;
using NewsFeedSystem.DataAccess.Configurations;
using NewsFeedSystem.DataAccess.Entities;

namespace NewsFeedSystem.DataAccess
{
    public class NewsFeedSystemDbContext : DbContext
    {
        public NewsFeedSystemDbContext(DbContextOptions<NewsFeedSystemDbContext> options) : base(options)
        {
        
        }

        public DbSet<AuthUser> AuthUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthUserConfiguration());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
