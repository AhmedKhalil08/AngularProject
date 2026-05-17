using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    internal class WishlistRepository :GenericRepository<Wishlist,int>,IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context)
        {
        }
        public override async Task<IReadOnlyList<Wishlist>> GetAllAsync()
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                    .ThenInclude(p => p.Images)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task ClearAllAsync(string userId)
        {
            var items = await _context.Wishlists
                .Where(w => w.UserId == userId && !w.IsDeleted)
                .ToListAsync();

            foreach (var item in items)
                item.IsDeleted = true;

            await _context.SaveChangesAsync();
        }
    }
}
