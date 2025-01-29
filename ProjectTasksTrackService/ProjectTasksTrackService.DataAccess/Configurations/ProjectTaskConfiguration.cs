using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTasksTrackService.DataAccess.Entities;

namespace ProjectTasksTrackService.DataAccess.Configurations
{
    public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
    {
        private const int MAX_NAME_LENGTH = 255;
        private const int MAX_URL_LENGTH = 2048;
        private const int DATETIME_LENGTH = 25;

        public void Configure(EntityTypeBuilder<ProjectTask> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.ProjectId).HasField("_projectId");
            builder.Property(t => t.Code).HasField("_code");
            builder.Property(t => t.RepeatsType).HasField("_repeatsType").IsRequired();
            builder.Property(t => t.RepeatInDays).HasField("_repeatInDays");

            builder.Property(t => t.Id).IsRequired();
            builder.Property(t => t.Code).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(t => t.Name).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(t => t.Url1).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(t => t.Url2).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(t => t.ImageUrl).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(t => t.CreatedDt).HasMaxLength(DATETIME_LENGTH);
            builder.Property(t => t.LastUpdateDt).HasMaxLength(DATETIME_LENGTH);
            builder.Property(t => t.DeadLineDt).HasMaxLength(DATETIME_LENGTH);
            builder.Property(t => t.DoneDt).HasMaxLength(DATETIME_LENGTH);

            builder.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.NoAction)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(t => t.ProjectId)
                .IsRequired(true);

            builder.HasOne(t => t.ProjectSubDivision)
                .WithMany(subDivision => subDivision.Tasks)
                .OnDelete(DeleteBehavior.NoAction)
                .HasPrincipalKey(subDivision => subDivision.Id)
                .HasForeignKey(t => t.ProjectSubDivisionId)
                .IsRequired(false);
        }
    }
}
