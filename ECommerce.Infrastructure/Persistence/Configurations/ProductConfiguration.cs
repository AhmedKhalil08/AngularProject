using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // 1. Primary Key
            builder.HasKey(p => p.Id);
            // 2. Properties
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Stock).IsRequired();
            builder.Property(p => p.IsActive).HasDefaultValue(true);
            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()"); 
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.NoAction);
            /* builder.HasOne(p => p.Seller)
                   .WithMany(s => s.Products)
                   .HasForeignKey(p => p.SellerId)
                   .OnDelete(DeleteBehavior.NoAction); 
            */
        }
    }
}