using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class WarehousesConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        private const int MAX_EMAIL_LENGTH = 254;
        private const int MAX_NAME_LENGTH = 255;
        private const int MAX_PHONE_LIST_LENGTH = 255;
        private const int MAX_URL_LENGTH = 2048;
        private const int MAX_SCHEDULE_LENGTH = 255;
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(w => w.Address).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(w => w.Email).HasMaxLength(MAX_EMAIL_LENGTH).IsRequired();
            builder.Property(w => w.Phone).HasMaxLength(MAX_PHONE_LIST_LENGTH).IsRequired();
            builder.Property(w => w.Url).HasMaxLength(MAX_URL_LENGTH).IsRequired();
            builder.Property(w => w.WorkSchedule).HasMaxLength(MAX_SCHEDULE_LENGTH).IsRequired();
        }
    }
}
