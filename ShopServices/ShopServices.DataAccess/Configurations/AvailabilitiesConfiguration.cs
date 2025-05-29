using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class AvailabilitiesConfiguration : IEntityTypeConfiguration<Availability>
    {
        public void Configure(EntityTypeBuilder<Availability> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.ProductId).IsRequired();
            builder.Property(a => a.Count).IsRequired();
        }
    }
}
