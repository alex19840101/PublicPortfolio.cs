﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopServices.DataAccess.Entities;

namespace ShopServices.DataAccess.Configurations
{
    internal class OrdersConfiguration : IEntityTypeConfiguration<Order>
    {
        private const int MAX_DIMENSIONS_LENGTH = 25;
        private const int MAX_NAME_LENGTH = 255;
        private const int MAX_CURRENCY_LENGTH = 5;
        private const int MAX_COMMENT_LENGTH = 255;

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Currency).HasMaxLength(MAX_CURRENCY_LENGTH).IsRequired();
            builder.Property(o => o.PaymentInfo).HasMaxLength(MAX_NAME_LENGTH).IsRequired();

            builder.Property(o => o.DeliveryAddress).HasMaxLength(MAX_NAME_LENGTH);
            builder.Property(o => o.ExtraInfo).HasMaxLength(MAX_NAME_LENGTH).IsRequired();

            builder.Property(o => o.MassInGrams).IsRequired();
            builder.Property(o => o.Dimensions).HasMaxLength(MAX_DIMENSIONS_LENGTH).IsRequired();
            builder.Property(o => o.PaymentInfo).HasMaxLength(MAX_NAME_LENGTH).IsRequired();
            builder.Property(o => o.Comment).HasMaxLength(MAX_COMMENT_LENGTH);

            //builder.Property(o => o.Buyer).IsRequired();
            //builder.Property(o => o.Positions).IsRequired();

            builder.HasMany(o => o.Products)
                .WithMany(p => p.Orders);

            builder.HasMany(o => o.Positions)
                .WithOne(op => op.Order).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.Buyer)
                .WithMany(b => b.Orders).IsRequired().OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(o => o.Delivery)
            //    .WithOne(delivery => delivery.Order).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
