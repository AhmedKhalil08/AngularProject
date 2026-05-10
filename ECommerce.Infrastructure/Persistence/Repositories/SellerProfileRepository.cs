using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    internal class SellerProfileRepository:GenericRepository<SellerProfile,int>,ISellerProfileRepository
    {
        public SellerProfileRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<SellerProfile> GetByUserIdAsync(string userId)
        {
            return await _context.SellerProfiles
                .FirstOrDefaultAsync(s => s.UserId == userId && !s.IsDeleted);
        }
    }
}
