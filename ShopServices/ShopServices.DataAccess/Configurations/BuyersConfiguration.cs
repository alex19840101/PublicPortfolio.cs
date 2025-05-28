using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class BuyersConfiguration : IEntityTypeConfiguration<Buyer>
    {
        private const int MAX_NAME_LENGTH = 255;
        private const int MAX_STRING_LENGTH = 255;
        private const int DATETIME_LENGTH = 25;

        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.HasKey(b => b.Id);
            builder.HasAlternateKey(b => b.Login);

            builder.Property(b => b.Id).HasField("_id").HasColumnType("int");
            builder.Property(b => b.Login).HasField("_login").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(b => b.Name).HasField("_name").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(b => b.Surname).HasField("_surname").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(b => b.Address).HasField("_address").HasMaxLength(MAX_STRING_LENGTH);
            builder.Property(b => b.Email).HasField("_email").HasMaxLength(MAX_STRING_LENGTH).IsRequired();
            builder.Property(b => b.PasswordHash).HasField("_passwordHash").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(b => b.GranterId).HasField("_granterId").HasColumnType("int");

            builder.Property(b => b.Nick).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(b => b.Phone).HasMaxLength(MAX_NAME_LENGTH);
            
            builder.Property(b => b.CreatedDt).HasMaxLength(DATETIME_LENGTH).IsRequired();
            builder.Property(b => b.LastUpdateDt).HasMaxLength(DATETIME_LENGTH);
        }
    }
}
