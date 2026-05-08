using ECommerce.Application.DTOs;
using ECommerce.Application.Features.OrderItems.Queries.GetOrderItemById;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

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
                    ZipCode = item.Address?.PostalCode,
                    Country = item.Address?.Country,
                    Id = item.AddressId,
                    Phone= item.Address?.Phone,
                    FullName = item.Address?.FullName,
                    IsDefault = item.Address?.IsDefault ?? false

                },
                PromoCode = new PromoCodeDto
                {
                    Id = item.PromoCodeId,
                    Code = item.PromoCode?.Code,
                    DiscountPercent = item.PromoCode?.DiscountPercent ?? 0,
                    MaxUsageCount = item.PromoCode?.MaxUsageCount ?? 0,
                    CurrentUsageCount = item.PromoCode?.CurrentUsageCount ?? 0,
                    ExpiryDate = item.PromoCode?.ExpiryDate ?? DateTime.MinValue,
                    IsActive = item.PromoCode?.IsActive ?? false
                }

            };
        }
    }
}
