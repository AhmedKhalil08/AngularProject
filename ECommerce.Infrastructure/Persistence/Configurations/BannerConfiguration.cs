using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class BannerConfiguration : IEntityTypeConfiguration<Banner>
    {

        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(a => a.ImageUrl)
               .IsRequired();
        }
    } 
}
