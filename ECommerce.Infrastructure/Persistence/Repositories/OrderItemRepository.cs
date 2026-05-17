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
    public class OrderItemRepository : GenericRepository<OrderItem, int>, IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task AddAsync(OrderItem entity)
        {
            if(entity!=null)
            {
                await _context.Set<OrderItem>().AddAsync(entity);
            }
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _context.Set<OrderItem>().Update(entity);
            }
        }

        public override async Task<IReadOnlyList<OrderItem>> GetAllAsync()
        {
            return await _context.Set<OrderItem>().ToListAsync();
        }

        public override async Task<OrderItem> GetByIdAsync(int id)
        {
            return await _context.Set<OrderItem>().FindAsync(id) ?? throw new KeyNotFoundException($"OrderItem with id {id} not found.");
        }

        public override Task UpdateAsync(OrderItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "OrderItem entity cannot be null.");
            }
            else
            {
                _context.Set<OrderItem>().Update(entity);
                return Task.CompletedTask;
            }
        }

    }
}
