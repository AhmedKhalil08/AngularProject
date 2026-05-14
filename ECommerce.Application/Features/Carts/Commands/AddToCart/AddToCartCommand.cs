using MediatR;

namespace ECommerce.Application.Features.Carts.Commands.AddToCart
{
    public class AddToCartCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
