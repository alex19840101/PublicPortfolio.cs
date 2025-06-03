using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class EmailNotificationsConfiguration : IEntityTypeConfiguration<EmailNotification>
    {
        private const int MAX_EMAIL_LENGTH = 254;
        private const int MAX_MESSAGE_LENGTH = 2048;

        public void Configure(EntityTypeBuilder<EmailNotification> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.EmailFrom).HasMaxLength(MAX_EMAIL_LENGTH).IsRequired();
            builder.Property(e  => e.EmailTo).HasMaxLength(MAX_EMAIL_LENGTH).IsRequired();
            builder.Property(e => e.Message).HasMaxLength(MAX_MESSAGE_LENGTH).IsRequired();
        }
    }
}
