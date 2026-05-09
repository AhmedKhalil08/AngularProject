using ECommerce.Application.DTOs;
using ECommerce.Application.Features.OrderItems.Queries.GetOrderItemById;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ECommerce.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler: IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _repository;
        public GetOrderByIdQueryHandler(IOrderRepository repository)
        {
            _repository = repository;
        }
        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return null;
            return new OrderDto
            {
                Id = item.Id,
                OrderDate = item.OrderDate,
                UserName=item.User.FullName,
                TotalAmount = item.TotalAmount,
                Status = item.Status,
                Notes = item.Notes,
               
                
                Address = new AddressDto
                {
                   FullName= item.ShippingAddress?.FullName?? string.Empty,
                   Street = item.ShippingAddress?.Street ?? string.Empty,
                   City = item.ShippingAddress?.City ?? string.Empty,
                   State = item.ShippingAddress?.State ?? string.Empty,
                   Country = item.ShippingAddress?.Country ?? string.Empty,
                   ZipCode = item.ShippingAddress?.ZipCode ?? string.Empty,
                   Phone = item.ShippingAddress?.Phone ?? string.Empty,
                   IsDefault = item.ShippingAddress?.IsDefault ?? true



                },
                PromoCode = new PromoCodeDto
                {
                    Id = item.PromoCodeId??0,
                    Code = item.PromoCode?.Code,
                    DiscountPercent = item.PromoCode?.DiscountPercent ?? 0,
                    MaxUsageCount = item.PromoCode?.MaxUsageCount ?? 0,
                    CurrentUsageCount = item.PromoCode?.CurrentUsageCount ?? 0,
                    ExpiryDate = item.PromoCode?.ExpiryDate ?? DateTime.MinValue,
                    
                }

            };
        }
    }
}
