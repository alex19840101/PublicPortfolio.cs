using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class OrderPositionsConfiguration : IEntityTypeConfiguration<OrderPosition>
    {
        private const int MAX_NAME_LENGTH = 255;
        private const int MAX_CURRENCY_LENGTH = 5;

        public void Configure(EntityTypeBuilder<OrderPosition> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.ArticleNumber).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(o => o.Brand).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(o => o.Name).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(o => o.Params).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(o => o.Currency).HasMaxLength(MAX_CURRENCY_LENGTH).IsRequired();
        }
    }
}
