using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.CartItems.Queries.GetAllCartItems
{
    public class GetAllCartItemsQuery : IRequest<List<CartItemDto>>
    {
    }
}
