using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class PricesConfiguration : IEntityTypeConfiguration<Price>
    {
        private const int MAX_CURRENCY_LENGTH = 5;
        private const int MAX_UNITS_NAME_LENGTH = 5;

        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.PricePerUnit).IsRequired();
            builder.Property(p => p.Currency).HasMaxLength(MAX_CURRENCY_LENGTH).IsRequired();
            builder.Property(p => p.Unit).HasMaxLength(MAX_UNITS_NAME_LENGTH).IsRequired();

        }
    }
}
