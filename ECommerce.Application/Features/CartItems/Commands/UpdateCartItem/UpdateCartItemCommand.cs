using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.CartItems.Commands.UpdateCartItem
{
    public class UpdateCartItemCommand : IRequest<CartItemDto>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
