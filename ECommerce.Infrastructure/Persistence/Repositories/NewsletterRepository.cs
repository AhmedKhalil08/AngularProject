using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class NewsletterRepository : GenericRepository<NewsletterSubscriber, int>, INewsletterRepository
    {
        public NewsletterRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<NewsletterSubscriber?> GetByEmailAsync(string email)
        {
            return await _context.NewsletterSubscribers
                .FirstOrDefaultAsync(n => n.Email == email);
        }
    }
}
