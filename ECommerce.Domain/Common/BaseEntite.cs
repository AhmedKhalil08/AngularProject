namespace ECommerce.Domain.Common
{
    public abstract class BaseEntite<T>
    {
        public T Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
