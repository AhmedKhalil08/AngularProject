using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class SellerProfileConfiguration : IEntityTypeConfiguration<SellerProfile>
    {
        public void Configure(EntityTypeBuilder<SellerProfile> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.StoreName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.StoreDescription)
                .HasMaxLength(1000);

            builder.Property(s => s.TotalEarnings)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(s => s.User)
                .WithOne(u => u.SellerProfile)
                .HasForeignKey<SellerProfile>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasAlternateKey(s => s.UserId);
            builder.HasMany(s => s.Products)
                .WithOne(p => p.Seller)
                .HasForeignKey(p => p.SellerId)
                .HasPrincipalKey(s => s.UserId); ;
            builder.HasAlternateKey(s => s.UserId);
        }
    }
}