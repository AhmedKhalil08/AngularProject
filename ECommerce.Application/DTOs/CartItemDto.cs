namespace ECommerce.Application.DTOs
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; } 
        public decimal UnitPrice { get; set; }   
        public int Quantity { get; set; }
        public decimal SubTotal => UnitPrice * Quantity; 
        public int StockQuantity { get; set; }
    }
}

