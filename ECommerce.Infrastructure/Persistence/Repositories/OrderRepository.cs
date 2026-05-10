using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Order, int>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task AddAsync(Order entity)
        {
            await _context.Set<Order>().AddAsync(entity);
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _context.Set<Order>().Update(entity);
            }
        }

        public override async Task<IReadOnlyList<Order>> GetAllAsync()
        {
            return await _context.Set<Order>().ToListAsync();
        }

        public override async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Set<Order>().FindAsync(id) ?? throw new KeyNotFoundException($"Order with id {id} not found.");
        }

        public override Task UpdateAsync(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Order entity cannot be null.");
            }
            else
            {
                _context.Set<Order>().Update(entity);
                return Task.CompletedTask;
            }
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Set<Order>().Where(o => o.UserId == userId).ToListAsync();
        }
       
    }
}
