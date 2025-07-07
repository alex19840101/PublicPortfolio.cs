using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class PhoneNotificationsConfiguration : IEntityTypeConfiguration<PhoneNotification>
    {
        private const int MAX_PHONE_LENGTH = 15;
        private const int MAX_SMS_MESSAGE_LENGTH = 160;
        private const int MAX_STRING_LENGTH = 255;

        public void Configure(EntityTypeBuilder<PhoneNotification> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.SmsFrom).HasMaxLength(MAX_PHONE_LENGTH).IsRequired();
            builder.Property(p => p.SmsTo).HasMaxLength(MAX_PHONE_LENGTH).IsRequired();
            builder.Property(p => p.Message).HasMaxLength(MAX_SMS_MESSAGE_LENGTH).IsRequired();

            builder.Property(p => p.Creator).HasMaxLength(MAX_STRING_LENGTH).IsRequired();
        }
    }
}
