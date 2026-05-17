using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class PromoCodeConfiguration: IEntityTypeConfiguration<PromoCode>
    {
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
            builder.HasKey(oi => oi.Id);
            builder.HasIndex(oi => oi.Code)
                   .IsUnique();
            builder.Property(oi => oi.Code)
           .IsRequired();

            builder.Property(oi=> oi.DiscountPercent).HasColumnType("decimal(18,2)");
        }
    }
}
