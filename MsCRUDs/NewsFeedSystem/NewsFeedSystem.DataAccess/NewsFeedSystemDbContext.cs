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
        public DbSet<News> News { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Topic> Topics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthUserConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new TagsConfiguration());
            modelBuilder.ApplyConfiguration(new TopicsConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
