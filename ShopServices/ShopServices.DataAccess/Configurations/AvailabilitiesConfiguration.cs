using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class AvailabilitiesConfiguration : IEntityTypeConfiguration<Availability>
    {
        private const int MAX_STRING_LENGTH = 255;
        
        public void Configure(EntityTypeBuilder<Availability> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.ProductId).IsRequired();
            builder.Property(a => a.Count).IsRequired();
            builder.Property(a => a.CityTownCode).IsRequired();
            builder.Property(a => a.PlaceName).IsRequired();
            builder.Property(a => a.PlaceName).HasMaxLength(MAX_STRING_LENGTH).IsRequired();
        }
    }
}
