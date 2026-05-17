using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.TrackingNumber).HasMaxLength(100);
            builder.Property(s => s.ShippingFee).HasColumnType("decimal(18,2)");
            builder.Property(s=>s.TotalAmount).HasColumnType("decimal(18,2)");
            // Relation with Order (If Order is deleted, delete shipments)
            builder.HasOne(s => s.Order)
                   .WithMany(o => o.Shipments)
                   .HasForeignKey(s => s.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relation with SellerProfile using UserId as Principal Key
            builder.HasOne(s => s.Seller)
                   .WithMany(sp => sp.Shipments)
                   .HasForeignKey(s => s.SellerId)
                   .HasPrincipalKey(sp => sp.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }

}

