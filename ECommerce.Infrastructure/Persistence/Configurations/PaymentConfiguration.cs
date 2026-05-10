using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration: IEntityTypeConfiguration<Payment>
    {

        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Amount)
            .IsRequired();

            builder.HasOne(c => c.Order)
                .WithOne(u => u.Payment)
                .HasForeignKey<Payment>(c => c.OrderId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
