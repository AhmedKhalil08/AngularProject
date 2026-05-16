using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class PromoCodeRepository: GenericRepository<PromoCode, int>, IPromoCodeRepository
    {
        public PromoCodeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task AddAsync(PromoCode entity)
        {
            await _context.Set<PromoCode>().AddAsync(entity);
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _context.Set<PromoCode>().Update(entity);
            }
        }

        public override async Task<IReadOnlyList<PromoCode>> GetAllAsync()
        {
            return await _context.Set<PromoCode>().ToListAsync();
        }

        public override async Task<PromoCode> GetByIdAsync(int id)
        {
            return await _context.Set<PromoCode>().FindAsync(id) ?? throw new KeyNotFoundException($"PromoCode with id {id} not found.");
        }

        public override Task UpdateAsync(PromoCode entity)
        {
                _context.Set<PromoCode>().Update(entity);
                return Task.CompletedTask;
            
        }
        public async Task<PromoCode> GetPromoCodeAsync(string code)
        {
            return await _context.Set<PromoCode>().FirstOrDefaultAsync(p => p.Code == code && !p.IsDeleted);
        }

    }
}
