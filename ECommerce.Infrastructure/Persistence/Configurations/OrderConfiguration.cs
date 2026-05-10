using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration: IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.OrderItems)
                .WithOne(ci => ci.Order)
                .HasForeignKey(ci => ci.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Payment)
               .WithOne(ci => ci.Order)
               .HasForeignKey<Payment>(ci => ci.OrderId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.PromoCode)
              .WithMany(u => u.Orders)
              .HasForeignKey(c => c.PromoCodeId)
              .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
