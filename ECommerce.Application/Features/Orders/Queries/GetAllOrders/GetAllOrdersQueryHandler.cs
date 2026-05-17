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
            
            var ordersEntity = await _repository.Table
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
                    ZipCode = order.ShippingAddress.ZipCode,
                    Phone = order.ShippingAddress.Phone
                },

                // مابينج الـ Items
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? "Unknown",
                    Quantity = item.Quantity,
                    Price = item.UnitPrice // تأكد إنها decimal في الـ DTO
                }).ToList(),

                // مابينج الدفع
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
