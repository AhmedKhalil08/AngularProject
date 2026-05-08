using ECommerce.Application.DTOs;
using ECommerce.Application.Features.CartItems.Commands.UpdateCartItem;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;

namespace ECommerce.Application.Features.OrderItems.Commands.UpdateOrderItem
{
    public class UpdateOrderItemCommand: IRequest<OrderItemDto>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
