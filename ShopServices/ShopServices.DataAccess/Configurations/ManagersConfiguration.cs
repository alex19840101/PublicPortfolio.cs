using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class ManagersConfiguration : IEntityTypeConfiguration<Manager>
    {
        private const int MAX_STRING_LENGTH = 255;

        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.Property(m => m.WorkStatus).HasMaxLength(MAX_STRING_LENGTH).IsRequired();
            builder.Property(m => m.ClientGroups).HasMaxLength(MAX_STRING_LENGTH);
            builder.Property(m => m.GoodsCategories).HasMaxLength(MAX_STRING_LENGTH);
            builder.Property(m => m.Services).HasMaxLength(MAX_STRING_LENGTH);
        }
    }
}
