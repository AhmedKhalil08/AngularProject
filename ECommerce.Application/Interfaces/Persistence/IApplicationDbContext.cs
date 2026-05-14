using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interfaces.Persistence
{
    public interface IApplicationDbContext
    {
        // KANDEL
        DbSet<Category> Categories { get; }
        DbSet<Product> Products { get; }
        DbSet<CartItem> CartItems { get; }
        DbSet<ProductImage> ProductImages { get; }
        DbSet<Review> Reviews { get; }
        DbSet<Shipment> Shipments { get; }

        // Ahmed 
        DbSet<ApplicationUser> Users { get; }
        DbSet<Address> Addresses { get; }
        DbSet<SellerProfile> SellerProfiles { get; }
        DbSet<Cart> Carts { get; }
        DbSet<Wishlist> Wishlists { get; }

        // Arwa 
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<Payment> Payments { get; }
        DbSet<PromoCode> PromoCodes { get; }
        DbSet<Banner> Banners { get; }

        // الميثود الأساسية للحفظ
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
