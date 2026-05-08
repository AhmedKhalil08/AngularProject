using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entites
{
    public class CartItem : BaseEntity<int>
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Product Product { get; set; }

        // public Cart Cart { get; set; }
    }
}
