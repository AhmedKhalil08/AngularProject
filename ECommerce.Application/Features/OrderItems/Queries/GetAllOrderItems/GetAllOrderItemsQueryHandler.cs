using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;

namespace ECommerce.Application.Features.OrderItems.Queries.GetAllOrderItems
{
    public class GetAllOrderItemsQueryHandler : IRequestHandler<GetAllOrderItemsQuery, List<OrderItemDto>>
    {
        private readonly IOrderItemRepository _repository;

        public GetAllOrderItemsQueryHandler(IOrderItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderItemDto>> Handle(GetAllOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllAsync();
            return items.Select(item => new OrderItemDto
            {
                Id = item.Id,
                Quantity = item.Quantity,
                ProductId = item.ProductId,
               
                OrderId = item.OrderId
            }).ToList();
        }
    }
}
