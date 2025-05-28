using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class EmailNotificationsConfiguration : IEntityTypeConfiguration<EmailNotification>
    {
        public void Configure(EntityTypeBuilder<EmailNotification> builder)
        {
            builder.HasKey(e => e.Id);
        }
    }
}
