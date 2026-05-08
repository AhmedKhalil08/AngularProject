using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities
{
    public class Review : BaseEntity<int>
    {
        public int ProductId { get; set; }

        public string UserId { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Product Product { get; set; }

         //public ApplicationUser User { get; set; }
    }
}
