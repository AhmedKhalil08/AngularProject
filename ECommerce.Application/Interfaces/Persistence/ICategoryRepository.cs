using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces.Persistence
{
    public interface ICategoryRepository : IGenericRepository<Category, int>
    {
    }
}
