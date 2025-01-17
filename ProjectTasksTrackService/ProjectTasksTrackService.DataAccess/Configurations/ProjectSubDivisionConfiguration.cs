using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTasksTrackService.DataAccess.Entities;

namespace ProjectTasksTrackService.DataAccess.Configurations
{
    public class ProjectSubDivisionConfiguration : IEntityTypeConfiguration<ProjectSubDivision>
    {
        private const int MAX_NAME_LENGTH = 255;
        private const int MAX_URL_LENGTH = 2048;
        private const int DATETIME_LENGTH = 25;

        public void Configure(EntityTypeBuilder<ProjectSubDivision> builder)
        {
            builder.HasKey(s => s.Id);
            
            builder.Property(s => s.Id).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(s => s.ProjectId).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(s => s.Name).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(s => s.Url1).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(s => s.Url2).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(s => s.ImageUrl).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(s => s.CreatedDt).HasMaxLength(DATETIME_LENGTH);
            builder.Property(s => s.LastUpdateDt).HasMaxLength(DATETIME_LENGTH);
            builder.Property(s => s.DeadLineDt).HasMaxLength(DATETIME_LENGTH);

            builder.HasMany(s => s.Tasks)
                .WithOne(t => t.ProjectSubDivision)
                .OnDelete(DeleteBehavior.NoAction)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(t => t.ProjectSubDivisionId)
                .IsRequired(false);
        }
    }
}
