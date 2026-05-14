using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;

namespace ECommerce.Application.Features.CartItems.Queries.GetAllCartItems
{
    public class GetAllCartItemsQueryHandler : IRequestHandler<GetAllCartItemsQuery, List<CartItemDto>>
    {
        private readonly ICartItemRepository _repository;

        public GetAllCartItemsQueryHandler(ICartItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CartItemDto>> Handle(GetAllCartItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllAsync();
            return items.Select(i => new CartItemDto
            {
                Id = i.Id,
                //CartId = i.CartId,
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList();
        }
    }
}