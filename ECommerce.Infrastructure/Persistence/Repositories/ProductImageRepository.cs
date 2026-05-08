using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entites;
using ECommerce.Infrastructure.Persistence.Contexts;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class ProductImageRepository : GenericRepository<ProductImage, int>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
