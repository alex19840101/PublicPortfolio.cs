using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class CategoriesConfiguration : IEntityTypeConfiguration<Category>
    {
        private const int MAX_NAME_LENGTH = 255;
        private const int MAX_URL_LENGTH = 2048;
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(c => c.Created).IsRequired();
            builder.Property(c => c.Archieved).IsRequired();

            builder.Property(c => c.Brand).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(c => c.Params).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(c => c.Url).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(c => c.ImageUrl).HasMaxLength(MAX_URL_LENGTH);
        }
    }
}
