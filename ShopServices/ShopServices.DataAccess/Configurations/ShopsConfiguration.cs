using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class ShopsConfiguration : IEntityTypeConfiguration<Shop>
    {
        private const int MAX_EMAIL_LENGTH = 254;
        private const int MAX_NAME_LENGTH = 255;
        private const int MAX_PHONE_LIST_LENGTH = 255;
        private const int MAX_URL_LENGTH = 2048;
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(s => s.Address).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(s => s.Email).HasMaxLength(MAX_EMAIL_LENGTH).IsRequired();
            builder.Property(s => s.Phone).HasMaxLength(MAX_PHONE_LIST_LENGTH).IsRequired();
            builder.Property(s => s.Url).HasMaxLength(MAX_URL_LENGTH).IsRequired();
        }
    }
}
