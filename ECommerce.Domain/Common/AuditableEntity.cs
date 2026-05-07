namespace ECommerce.Domain.Common
{
    public abstract class AuditableEntity<TId> : BaseEntite<TId>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        //public DateTime? UpdatedAt { get; set; }
        //public bool IsDeleted { get; set; } // عشان الـ Soft Delete
    }
}
