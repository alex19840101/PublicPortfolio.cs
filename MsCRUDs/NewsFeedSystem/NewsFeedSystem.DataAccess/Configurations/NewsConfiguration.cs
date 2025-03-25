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
    public class NewsConfiguration : IEntityTypeConfiguration<News>
    {
        private const int MAX_NAME_LENGTH = 255;
        private const int DATETIME_LENGTH = 25;
        private const int MAX_TEXT_LENGTH = 4095;
        private const int MAX_URL_LENGTH = 2048;

        public void Configure(EntityTypeBuilder<News> builder)
        {
            builder.HasKey(n => n.Id);
           
            builder.Property(n => n.Id).HasField("_id");
            builder.Property(n => n.Headline).HasField("_headLine").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(n => n.Text).HasField("_text").HasMaxLength(MAX_TEXT_LENGTH).IsRequired();

            builder.Property(n => n.URL).HasField("_url").HasMaxLength(MAX_URL_LENGTH);
            builder.Property(n => n.Author).HasField("_author").HasMaxLength(MAX_URL_LENGTH);
            
            
            builder.Property(n => n.Tags).HasField("_tags").IsRequired();
            builder.Property(n => n.Tags).HasField("_topics").IsRequired();

            builder.Property(n => n.Created).HasMaxLength(DATETIME_LENGTH).IsRequired();
            builder.Property(n => n.Updated).HasMaxLength(DATETIME_LENGTH);

            //TODO: связи NewsConfiguration
        }
    }
}
