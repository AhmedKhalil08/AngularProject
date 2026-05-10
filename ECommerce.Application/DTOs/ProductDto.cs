using ECommerce.Domain.Entities;

namespace ECommerce.Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public List<string> ImageUrls { get; set; }

        public string SellerName { get; set; }

        public string storeDes {  get; set; }

    }

}
