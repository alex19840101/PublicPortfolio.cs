using Microsoft.EntityFrameworkCore;
using ProjectTasksTrackService.DataAccess.Configurations;
using ProjectTasksTrackService.DataAccess.Entities;

namespace ProjectTasksTrackService.DataAccess
{
    public class ProjectTasksTrackServiceDbContext : DbContext
    {
        public ProjectTasksTrackServiceDbContext(DbContextOptions<ProjectTasksTrackServiceDbContext> options) : base(options)
        {
        }

        public DbSet<AuthUser> AuthUsers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectSubDivision> ProjectSubDivisions { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthUserConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectSubDivisionConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectTaskConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
