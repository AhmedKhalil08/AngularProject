using MediatR;

namespace ECommerce.Application.Features.CartItems.Commands.DeleteCartItem
{
    public class DeleteCartItemCommand : IRequest<bool>
    {
        public int CartItemId { get; set; }
    }
}
