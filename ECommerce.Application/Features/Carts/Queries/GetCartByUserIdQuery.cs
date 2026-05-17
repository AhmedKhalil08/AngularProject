using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Carts.Queries
{
    public class GetCartByUserIdQuery : IRequest<CartDto>
    {
    }
}
