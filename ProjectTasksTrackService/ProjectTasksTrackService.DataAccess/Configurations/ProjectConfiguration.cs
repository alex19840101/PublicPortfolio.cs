using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTasksTrackService.DataAccess.Entities;

namespace ProjectTasksTrackService.DataAccess.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        private const int MAX_NAME_LENGTH = 255;
        private const int MAX_URL_LENGTH = 2048;
        private const int DATETIME_LENGTH = 25;

        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);
            builder.HasAlternateKey(p => p.Code);

            builder.Property(p => p.Code).HasField("_code");
            builder.Property(p => p.Id).HasField("_id");

            builder.Property(p => p.Code).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(p => p.Url).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(p => p.ImageUrl).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(p => p.CreatedDt).HasMaxLength(DATETIME_LENGTH);
            builder.Property(p => p.LastUpdateDt).HasMaxLength(DATETIME_LENGTH);
            builder.Property(p => p.DeadLineDt).HasMaxLength(DATETIME_LENGTH);
            builder.Property(p => p.DoneDt).HasMaxLength(DATETIME_LENGTH);

            builder.HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .OnDelete(DeleteBehavior.NoAction)
                //.HasPrincipalKey(p => p.Id)
                //.HasForeignKey(t => t.ProjectId)
                .IsRequired(false);

            builder.HasMany(p => p.ProjectSubDivisions)
                .WithOne(subDivision => subDivision.Project)
                .OnDelete(DeleteBehavior.NoAction)
                //.HasPrincipalKey(p => p.Id)
                //.HasForeignKey(subDivision => subDivision.ProjectId)
                .IsRequired(false);
        }
    }
}
