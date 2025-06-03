using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class ProductsConfiguration : IEntityTypeConfiguration<Product>
    {
        private const int MAX_STRING_LENGTH = 255;
        private const int MAX_URL_LENGTH = 2048;
        private const int MAX_DIMENSIONS_LENGTH = 25;

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ArticleNumber).HasMaxLength(MAX_STRING_LENGTH);
            builder.Property(p => p.Parameters).HasMaxLength(MAX_STRING_LENGTH);
            builder.Property(p => p.Brand).HasMaxLength(MAX_STRING_LENGTH);
            builder.Property(p => p.Name).HasMaxLength(MAX_STRING_LENGTH);
            builder.Property(p => p.Url).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(p => p.ImageUrl).HasMaxLength(MAX_URL_LENGTH);
            builder.Property(p => p.Dimensions).HasMaxLength(MAX_DIMENSIONS_LENGTH).IsRequired();
        }
    }
}
