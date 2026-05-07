using ECommerce.Domain.Entites;

namespace ECommerce.Application.Interfaces.Persistence
{
    public interface ICartItemRepository : IGenericRepository<CartItem,int>
    {
    }
}
