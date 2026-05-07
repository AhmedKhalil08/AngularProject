using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.CartItems.Commands.CreateCartItem
{
    public class CreateCartItemCommand : IRequest<CartItemDto>
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
