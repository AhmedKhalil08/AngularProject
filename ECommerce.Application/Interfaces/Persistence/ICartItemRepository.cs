using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces.Persistence
{
    public interface ICartItemRepository : IGenericRepository<CartItem,int>
    {
    }
}
