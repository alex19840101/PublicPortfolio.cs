using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    public class EmployeesConfiguration : IEntityTypeConfiguration<Employee>
    {
        private const int MAX_NAME_LENGTH = 255;
        private const int DATETIME_LENGTH = 25;

        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasAlternateKey(p => p.Login);

            builder.Property(t => t.Id).HasField("_id").HasColumnType("int");
            builder.Property(t => t.Login).HasField("_login").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(t => t.Name).HasField("_name").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(t => t.Surname).HasField("_surname").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(t => t.Email).HasField("_email").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(t => t.PasswordHash).HasField("_passwordHash").HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(t => t.GranterId).HasField("_granterId").HasColumnType("int");

            builder.Property(t => t.Nick).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(t => t.Phone).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(t => t.Role).HasMaxLength(MAX_NAME_LENGTH);
            
            builder.Property(t => t.CreatedDt).HasMaxLength(DATETIME_LENGTH).IsRequired();
            builder.Property(t => t.LastUpdateDt).HasMaxLength(DATETIME_LENGTH);
        }
    }
}
