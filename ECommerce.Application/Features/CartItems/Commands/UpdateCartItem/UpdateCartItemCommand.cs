using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.CartItems.Commands.UpdateCartItem
{
    public class UpdateCartItemCommand : IRequest<bool>
    {
        public int CartItemId { get; set; }

        // The NEW quantity the user wants (if 0, it means delete the item)
        public int Quantity { get; set; }
    }
}
