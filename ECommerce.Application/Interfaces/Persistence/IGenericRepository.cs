namespace ECommerce.Application.Interfaces.Persistence
{
    public interface IGenericRepository<T,Tkey> where T : class
    {
        Task<T> GetByIdAsync(Tkey id);
        Task<IReadOnlyList<T>> GetAllAsync();
         IQueryable<T> Table {  get; } 
        Task AddAsync(T entity);
        Task DeleteAsync(Tkey id);
        Task UpdateAsync(T entity);
    }
}
