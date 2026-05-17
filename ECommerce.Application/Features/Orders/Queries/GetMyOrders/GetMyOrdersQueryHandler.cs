using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Orders.Queries.GetMyOrders
{
    public class GetMyOrdersQueryHandler: IRequestHandler<GetMyOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrentUserService _currentUserService;
        

        public GetMyOrdersQueryHandler(IOrderRepository orderRepository, ICurrentUserService currentUserService)
        {
            _orderRepository = orderRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<OrderDto>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Must be logged in.");
            //var userId = "620309fb-6d65-4fb5-ba45-14fb66df6bf9";

            var ordersEntity = await _orderRepository.Table
         .Where(o => o.UserId == userId)
         .Include(o => o.User)
         .Include(o => o.ShippingAddress)
         .Include(o => o.Payment)
         .Include(o => o.PromoCode)
         .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
            
         .ToListAsync(cancellationToken);

         
            var ordersDto = ordersEntity.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Notes = order.Notes,
                UserName = order.User?.UserName ?? "N/A",
                IsDeleted = order.IsDeleted,

                Address = order.ShippingAddress == null ? null : new AddressDto
                {
                    Id = order.ShippingAddress.Id,
                    FullName = order.ShippingAddress.FullName,
                    Street = order.ShippingAddress.Street,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    Country = order.ShippingAddress.Country,
                    Phone = order.ShippingAddress.Phone
                },

                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? "Unknown",
                    Quantity = item.Quantity,
                    Price = item.UnitPrice
                }).ToList(),

                Payment = order.Payment == null ? null : new PaymentDto
                {
                    Id = order.Payment.Id,
                    Amount = order.Payment.Amount,
                    UserName = order.User?.UserName ?? "N/A",
                    TransactionId = order.Payment.TransactionId ?? "",
                    Method = order.Payment.Method,
                    Status = order.Payment.Status,
                    PaidAt = order.Payment.PaidAt
                }
            }).ToList();

            return ordersDto;
        }
    }
}
