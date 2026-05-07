using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CartItems.Queries.GetCartItemById
{
    public class GetCartItemByIdQueryHandler : IRequestHandler<GetCartItemByIdQuery, CartItemDto>
    {
        private readonly ICartItemRepository _repository;

        public GetCartItemByIdQueryHandler(ICartItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<CartItemDto> Handle(GetCartItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return null;

            return new CartItemDto
            {
                Id = item.Id,
                CartId = item.CartId,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
        }
    }
}
