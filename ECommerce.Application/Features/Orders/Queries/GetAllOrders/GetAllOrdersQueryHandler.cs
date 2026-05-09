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
                UserName=item.User.FullName,
                TotalAmount = item.TotalAmount,
                Status = item.Status,
                Notes = item.Notes,
               
                Address = new AddressDto
                {
                    Street = item.ShippingAddress?.Street,
                    City = item.ShippingAddress?.City,
                    State = item.ShippingAddress?.State,
                    ZipCode = item.ShippingAddress?.ZipCode,
                    Country = item.ShippingAddress?.Country,
                    Phone = item.ShippingAddress?.Phone,
                    IsDefault = item.ShippingAddress?.IsDefault ?? true
                },
                PromoCode = new PromoCodeDto
                {
                    Id = item.PromoCodeId ?? 0,
                    Code = item.PromoCode?.Code,
                    DiscountPercent = item.PromoCode?.DiscountPercent ?? 0,
                    MaxUsageCount = item.PromoCode?.MaxUsageCount ?? 0,
                    CurrentUsageCount = item.PromoCode?.CurrentUsageCount ?? 0,
                    ExpiryDate = item.PromoCode?.ExpiryDate ?? DateTime.MinValue
                },


            }).ToList();
        }
    }
}
