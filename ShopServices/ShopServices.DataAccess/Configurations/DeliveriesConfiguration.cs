using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class DeliveriesConfiguration : IEntityTypeConfiguration<Delivery>
    {
        private const int MAX_DIMENSIONS_LENGTH = 25;
        private const int MAX_NAME_LENGTH = 255;

        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Address).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(d => d.PaymentInfo).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(d => d.MassInGrams).IsRequired();
            builder.Property(d => d.Dimensions).HasMaxLength(MAX_DIMENSIONS_LENGTH).IsRequired();
        }
    }
}
