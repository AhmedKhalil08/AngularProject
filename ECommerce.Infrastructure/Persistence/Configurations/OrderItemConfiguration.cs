using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
   public class OrderItemConfiguration: IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Quantity)
            .IsRequired();

            builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasOne(c => c.Order)
                .WithMany(u => u.OrderItems)
                .HasForeignKey(c => c.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Product)
               .WithMany()
              .HasForeignKey(c => c.ProductId)
              .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(oi => oi.Shipment)
                   .WithMany(s => s.OrderItems)
                   .HasForeignKey(oi => oi.ShipmentId)
                   .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
