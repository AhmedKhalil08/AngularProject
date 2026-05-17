using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces.Persistence
{
    public interface IShipmentRepository : IGenericRepository<Shipment, int>
    {
        Task<IEnumerable<Shipment>> GetShipmentsBySellerIdAsync(string sellerId);

        Task<IEnumerable<Shipment>> GetByOrderIdAsync(int orderId);
    }
}
