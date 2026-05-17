using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Infrastructure.Persistence.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Quantity)
                   .IsRequired();

            builder.HasOne(ci => ci.Product)       // CartItem has one Product
            .WithMany(p => p.CartItems)     //
                                            //.HasForeignKey(ci => ci.ProductId) // The FK is ProductId
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ci => ci.Cart)
                   .WithMany(c => c.CartItems)
                   .HasForeignKey(ci => ci.CartId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}