using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. خاصية الـ Table لاستخدامها مع Mapster (ProjectToType)
        // لاحظ استخدام AsNoTracking عشان السرعة في الـ Queries
        public IQueryable<T> Table => _context.Set<T>().AsNoTracking();

        public virtual async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }
        public virtual async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }
        public virtual async Task<T> GetByIdAsync(TKey id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public virtual Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }
        public async Task<IEnumerable<T>> GetByConditionAsync(
            Expression<Func<T, bool>> expression,
            string? includeProperties = null,
            bool trackChanges = true)
        {
            // 1. Apply the condition (WHERE clause)
            IQueryable<T> query = _context.Set<T>().Where(expression);
            // 2. Apply Includes if any are provided
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                // Split by comma in case there are multiple includes (e.g., "CartItems,CartItems.Product")
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            // 3. Execute the query and return the result
            return await query.ToListAsync();
        }
    }
}