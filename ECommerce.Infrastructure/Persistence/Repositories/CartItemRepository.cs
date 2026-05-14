using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class CartItemRepository : GenericRepository<CartItem, int>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext context) : base(context)
        {
            
        }
        public override async Task DeleteAsync(int id)
        {
            await _context.Set<CartItem>()
                          .Where(c => c.Id == id)
                          .ExecuteDeleteAsync();
        }
    }
}
