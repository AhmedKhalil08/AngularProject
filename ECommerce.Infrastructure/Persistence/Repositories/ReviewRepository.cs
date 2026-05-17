using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class ReviewRepository : GenericRepository<Review, int>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<Review>> GetByUserIdAsync(string userId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId && !r.IsDeleted)
                .ToListAsync();
        }
    }
}
