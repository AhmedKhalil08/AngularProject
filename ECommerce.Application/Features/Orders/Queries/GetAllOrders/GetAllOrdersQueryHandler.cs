using ECommerce.Application.DTOs;
using ECommerce.Application.Features.OrderItems.Queries.GetAllOrderItems;
using ECommerce.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ECommerce.Application.Features.Orders.Queries.GetAllOrders
{
    public class GetAllOrdersQueryHandler: IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _repository;

        public GetAllOrdersQueryHandler(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllAsync();
            return items.Select(item => new OrderDto
            {
                Id= item.Id,
                OrderDate = item.OrderDate,
                CustomerId = item.CustomerId,
                TotalAmount = item.TotalAmount,
                Status = item.Status,
                Notes = item.Notes,
                UserName = item.UserName,
                Address = new AddressDto
                {
                    Street = item.Address?.Street,
                    City = item.Address?.City,
                    State = item.Address?.State,
                    PostalCode = item.Address?.PostalCode,
                    Country = item.Address?.Country
                },
                PromoCode = new PromoCodeDto
                {
                    Id = item.PromoCodeId,
                    Code = item.PromoCode?.Code,
                    DiscountAmount = item.PromoCode?.DiscountAmount ?? 0
                },


            }).ToList();
        }
    }
}
