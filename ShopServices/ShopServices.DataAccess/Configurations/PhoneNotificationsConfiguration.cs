using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class PhoneNotificationsConfiguration : IEntityTypeConfiguration<PhoneNotification>
    {
        public void Configure(EntityTypeBuilder<PhoneNotification> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
