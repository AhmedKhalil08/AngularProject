using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class ShipmentRepository : GenericRepository<Shipment, int>, IShipmentRepository
    {

        public ShipmentRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Shipment>> GetShipmentsBySellerIdAsync(string sellerId)
        {
            return await _context.Shipments
                .Where(s => s.SellerId == sellerId)
                .ToListAsync();
        }
        public override async Task<Shipment> GetByIdAsync(int id)
        {
            return await _context.Shipments
                .Include(s => s.OrderItems) // 
                .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<IEnumerable<Shipment>> GetByOrderIdAsync(int orderId)
        {
            return await _context.Shipments
                .Where(s => s.OrderId == orderId)
                .Include(s=>s.OrderItems)
                .ToListAsync();
        }
        }
}
