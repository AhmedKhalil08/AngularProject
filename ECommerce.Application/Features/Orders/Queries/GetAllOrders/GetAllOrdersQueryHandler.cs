using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _repository;

        public GetAllOrdersQueryHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {

            var orders = await _repository
                .Table
                .ProjectToType<OrderDto>()
                .ToListAsync(cancellationToken);
            return orders;
        }
    }
}
