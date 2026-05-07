namespace ECommerce.Application.Interfaces.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task AddAsync(T entity);
        void DeleteAsync(int id);
        void UpdateAsync(T entity);
    }
}
