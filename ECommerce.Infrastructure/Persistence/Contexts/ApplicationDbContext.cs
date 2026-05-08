using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ECommerce.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        //KANDEL
        public DbSet<Domain.Entites.Category> Categories { get; set; }
        public DbSet<Domain.Entites.Product> Products { get; set; }
        public DbSet<Domain.Entites.CartItem> CartItems { get; set; }
        public DbSet<Domain.Entites.ProductImage> ProductImages { get; set; }
        public DbSet<Domain.Entites.Review> Reviews { get; set; }

        // Ahmed 
        public DbSet<Address> Addresses { get; set; }
        public DbSet<SellerProfile> SellerProfiles { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        // Arwa 
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Banner> Banners { get; set; }

    }
}
