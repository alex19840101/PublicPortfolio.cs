using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            builder.Property(t => t.Id).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(t => t.ProjectId).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(t => t.Name).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(t => t.Url1).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(t => t.Url2).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(t => t.ImageUrl).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(t => t.CreatedDt).HasMaxLength(DATETIME_LENGTH);
            builder.Property(t => t.LastUpdateDt).HasMaxLength(DATETIME_LENGTH);
            builder.Property(t => t.DeadLineDt).HasMaxLength(DATETIME_LENGTH);
        }
    }
}
