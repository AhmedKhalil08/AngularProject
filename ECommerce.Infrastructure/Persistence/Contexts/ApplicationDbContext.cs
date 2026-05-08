using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Domain.Entites.Category> Categories { get; set; }
        public DbSet<Domain.Entites.Product> Products { get; set; }
        public DbSet<Domain.Entites.CartItem> CartItems { get; set; }
        public DbSet<Domain.Entites.ProductImage> ProductImages { get; set; }
        public DbSet<Domain.Entites.Review> Reviews { get; set; }

    }
}
