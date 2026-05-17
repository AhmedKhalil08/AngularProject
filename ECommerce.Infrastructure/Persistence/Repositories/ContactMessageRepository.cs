using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class ContactMessageRepository : GenericRepository<ContactMessage, int>, IContactMessageRepository
    {
        public ContactMessageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<ContactMessage>> GetAllOrderedAsync()
        {
            return await _context.ContactMessages
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }
    }
}
