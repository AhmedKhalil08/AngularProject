using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Banners.Queries.GetBannerById;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.OrderItems.Queries.GetOrderItemById
{
    public class GetOrderItemByIdQueryHandler: IRequestHandler<GetOrderItemByIdQuery, OrderItemDto>
    {
        private readonly IOrderItemRepository _repository;
        public GetOrderItemByIdQueryHandler(IOrderItemRepository repository)
        {
            _repository = repository;
        }
        public async Task<OrderItemDto> Handle(GetOrderItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return null;
            return new OrderItemDto
            {
                Id = item.Id,
                Quantity = item.Quantity,
                ProductId = item.ProductId,
               
                OrderId = item.OrderId

            };
        }
    }
}
