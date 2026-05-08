using ECommerce.Domain.Common;

namespace ECommerce.Domain.Entities
{
    public class Category : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new HashSet<Category>();
        public ICollection<Product> Products { get; set; } = new   HashSet<Product>();
    }
}
