using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product, int>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Set<Product>()
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public override async Task DeleteAsync(int id)
        {
            await _context.Set<Product>()
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsDeleted, true));
        }

        public async Task<bool> IsUserOwnerOfProductAsync(int productId, string userId)
        {
            var product = await _context.Set<Product>()
                .FirstOrDefaultAsync(p => p.Id == productId && p.SellerId == userId);
            return product != null;
        }
    }
}