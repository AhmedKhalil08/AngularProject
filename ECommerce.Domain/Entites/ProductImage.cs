using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entites
{
    public class ProductImage : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }

        // Navigation Property
        public Product Product { get; set; }
    }
}
