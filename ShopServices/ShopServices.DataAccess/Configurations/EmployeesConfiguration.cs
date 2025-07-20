using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class EmployeesConfiguration : IEntityTypeConfiguration<Employee>
    {
        private const int MAX_EMAIL_LENGTH = 254;
        private const int MAX_NAME_LENGTH = 255;
        private const int MAX_STRING_LENGTH = 255;
        private const int DATETIME_LENGTH = 25;

        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasAlternateKey(e => e.Login);

            builder.Property(e => e.Id).HasField("_id").HasColumnType("int");
            builder.Property(e => e.Login).HasField("_login").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(e => e.Name).HasField("_name").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(e => e.Surname).HasField("_surname").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(e => e.Address).HasField("_address").HasMaxLength(MAX_STRING_LENGTH).IsRequired();
            builder.Property(e => e.Email).HasField("_email").HasMaxLength(MAX_EMAIL_LENGTH).IsRequired();
            builder.Property(e => e.PasswordHash).HasField("_passwordHash").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(e => e.GranterId).HasField("_granterId").HasColumnType("int");

            builder.Property(e => e.Nick).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(e => e.Phone).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(e => e.TelegramChatId).HasField("_telegramChatId");
            builder.Property(e => e.NotificationMethods).HasField("_notificationMethods");

            builder.Property(e => e.Role).HasMaxLength(MAX_NAME_LENGTH);
            
            builder.Property(e => e.CreatedDt).HasMaxLength(DATETIME_LENGTH).IsRequired();
            builder.Property(e => e.LastUpdateDt).HasMaxLength(DATETIME_LENGTH);
        }
    }
}
