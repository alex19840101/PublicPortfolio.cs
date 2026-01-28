using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class TradesConfiguration : IEntityTypeConfiguration<Trade>
    {
        private const int MAX_COMMENT_LENGTH = 255;
        private const int MAX_CURRENCY_LENGTH = 5;
        private const int MAX_EXTRA_INFO_LENGTH = 255;
        private const int MAX_PAYMENT_INFO_LENGTH = 255;
        private const int MAX_REFUND_INFO_LENGTH = 255;
        public void Configure(EntityTypeBuilder<Trade> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Currency).HasMaxLength(MAX_CURRENCY_LENGTH).IsRequired();
            builder.Property(t => t.PaymentInfo).HasMaxLength(MAX_PAYMENT_INFO_LENGTH).IsRequired();
            builder.Property(t => t.ExtraInfo).HasMaxLength(MAX_EXTRA_INFO_LENGTH);
            builder.Property(t => t.Comment).HasMaxLength(MAX_COMMENT_LENGTH);
            builder.Property(t => t.RefundInfo).HasMaxLength(MAX_REFUND_INFO_LENGTH);

            //builder.Property(t => t.Buyer).IsRequired();
            //builder.Property(t => t.Positions).IsRequired();

            builder.HasMany(t => t.Products)
                .WithMany(p => p.Trades);

            builder.HasMany(t => t.Positions)
                .WithOne(op => op.Trade).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.Buyer)
                .WithMany(b => b.Trades).IsRequired().OnDelete(DeleteBehavior.NoAction);
        }
    }
}
