using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class BannerRepository : GenericRepository<Banner, int>, IBannerRepository
    {
        public BannerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task AddAsync(Banner entity)
        {
            if (entity != null)
            {
                await _context.Set<Banner>().AddAsync(entity);
            }
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _context.Set<Banner>().Update(entity);
            }
        }

        public override async Task<IReadOnlyList<Banner>> GetAllAsync()
        {
            return await _context.Set<Banner>().ToListAsync();
        }

        public override async Task<Banner> GetByIdAsync(int id)
        {
            return await _context.Set<Banner>().FindAsync(id)?? throw new KeyNotFoundException($"Banner with id {id} not found.");
        }

        public override Task UpdateAsync(Banner entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Banner entity cannot be null.");

            }
            else
            {
                _context.Set<Banner>().Update(entity);
                return Task.CompletedTask;
            }
        }

    }
}
