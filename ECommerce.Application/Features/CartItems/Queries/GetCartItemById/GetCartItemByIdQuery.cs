using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.CartItems.Queries.GetCartItemById
{
    public class GetCartItemByIdQuery : IRequest<CartItemDto>
    {
        public int Id { get; set; }
    }
}
