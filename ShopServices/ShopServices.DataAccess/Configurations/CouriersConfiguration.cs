using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class CouriersConfiguration : IEntityTypeConfiguration<Courier>
    {
        private const int MAX_AREA_LENGTH = 255;
        private const int MAX_SCHEDULE_LENGTH = 255;
        private const int MAX_DRIVERS_CATEGORY_LENGTH = 3;
        private const int MAX_TRANSPORT_LENGTH = 255;
        public void Configure(EntityTypeBuilder<Courier> builder)
        {
            builder.Property(c => c.DriverLicenseCategory).HasMaxLength(MAX_DRIVERS_CATEGORY_LENGTH);
            builder.Property(c => c.Transport).HasMaxLength(MAX_TRANSPORT_LENGTH).IsRequired();
            builder.Property(c => c.Areas).HasMaxLength(MAX_AREA_LENGTH).IsRequired();
            builder.Property(c => c.DeliveryTimeSchedule).HasMaxLength(MAX_SCHEDULE_LENGTH).IsRequired();
        }
    }
}
