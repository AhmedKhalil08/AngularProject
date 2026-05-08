using ECommerce.Domain.Entites;

namespace ECommerce.Application.Interfaces.Persistence
{
    public interface IProductRepository : IGenericRepository<Product,int>
    {
        Task<IReadOnlyList<Product>> GetProductsByCategoryIdAsync(int categoryId);
    }

}
