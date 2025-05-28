using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class WarehousesConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.HasKey(w => w.Id);
        }
    }
}
