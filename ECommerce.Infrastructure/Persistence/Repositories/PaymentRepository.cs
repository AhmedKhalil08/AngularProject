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
    public class PaymentRepository : GenericRepository<Payment, int>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task AddAsync(Payment entity)
        {
            await _context.Set<Payment>().AddAsync(entity);
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                _context.Set<Payment>().Update(entity);
            }
        }

        public override async Task<IReadOnlyList<Payment>> GetAllAsync()
        {
            return await _context.Set<Payment>().ToListAsync();
        }

        public override async Task<Payment> GetByIdAsync(int id)
        {
            return await _context.Set<Payment>().FindAsync(id) ?? throw new KeyNotFoundException($"Payment with id {id} not found.");
        }

        public override Task UpdateAsync(Payment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Payment entity cannot be null.");
            }
            else
            {
                _context.Set<Payment>().Update(entity);
                return Task.CompletedTask;
            }
        }

    }
}
