using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NewsFeedSystem.DataAccess.Entities;

namespace NewsFeedSystem.DataAccess.Configurations
{
    public class TopicsConfiguration : IEntityTypeConfiguration<Topic>
    {
        private const int MAX_NAME_LENGTH = 255;

        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).HasField("_id");
            builder.Property(t => t.Name).HasField("_name").HasMaxLength(MAX_NAME_LENGTH).IsRequired();

            /* Многие ко многим (жесткие проверки и связи в БД избыточны в данном пет-проекте):
            builder.HasMany(t => t.News)
                .WithMany(news => news.Topics)
                .UsingEntity(tn => tn.ToTable("NewsTopics"));
            */
        }
    }
}
