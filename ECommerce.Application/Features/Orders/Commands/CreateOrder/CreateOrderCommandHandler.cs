using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IOrderRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Here you would typically add logic to create the order in the database
            // For this example, we'll just return a new OrderDto with the provided data
            var order = new Order
            {
                PromoCode = new PromoCode { Code = request.PromoCode },
                UserId = new UserDto { Id = request.Id }.Id,
                User= new ApplicationUser { Id = new UserDto { Id = request.Id }.Id }, 
                OrderItems = request.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Product= new Product { Price = item.Product.Price  }
                }).ToList(),
                TotalAmount = CalculateTotalAmount(request.OrderItems),
                Payment = new Payment
                {
                    Amount = CalculateTotalAmount(request.OrderItems),
                    Method = request.Payment.Method,
                    
                },
              ShippingAddress = new Address
                    {
                        FullName = request.Address.FullName,
                        Street = request.Address.Street,
                        City = request.Address.City,
                        State = request.Address.State,
                        Country = request.Address.Country,
                        ZipCode = request.Address.ZipCode,
                        Phone = request.Address.Phone,
                        IsDefault = request.Address.IsDefault
                    },
            };
            await _repository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();


            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Notes = order.Notes,
                UserName = order.User.UserName,
                Address = new AddressDto()
                {
                    Id= order.ShippingAddress?.Id ?? 0,
                    FullName = order.ShippingAddress?.FullName ?? string.Empty,
                    Street = order.ShippingAddress?.Street ?? string.Empty,
                    City = order.ShippingAddress?.City ?? string.Empty,
                    State = order.ShippingAddress?.State ?? string.Empty,
                    Country = order.ShippingAddress?.Country ?? string.Empty,
                    ZipCode = order.ShippingAddress?.ZipCode ?? string.Empty,
                    Phone = order.ShippingAddress?.Phone ?? string.Empty,
                    IsDefault = order.ShippingAddress?.IsDefault ?? true

                },
                PromoCode = new PromoCodeDto() {
                        Id = order.PromoCodeId ?? 0,
                        Code = order.PromoCode?.Code,
                        DiscountPercent = order.PromoCode?.DiscountPercent ?? 0,
                        MaxUsageCount = order.PromoCode?.MaxUsageCount ?? 0,
                        CurrentUsageCount = order.PromoCode?.CurrentUsageCount ?? 0,
                        ExpiryDate = order.PromoCode?.ExpiryDate ?? DateTime.MinValue

                },
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    Quantity = item.Quantity,
                    Price = (int)item.Product.Price,
                }).ToList(),
                Payment = new PaymentDto()
                {
                        Id = order.Payment?.Id ?? 0,
                        Amount = order.Payment?.Amount ?? 0,
                        Method = order.Payment?.Method ?? PaymentMethod.CreditCard,
                       TransactionId = order.Payment?.TransactionId ?? string.Empty
                }
            };
        }
        private decimal CalculateTotalAmount(List<OrderItem> orderItems)
        {
            // Simulate total amount calculation based on order items
            decimal total = 0;
            foreach (var item in orderItems)
            {
                total += item.Quantity * item.Product.Price; // Assume each item has a Price property
            }
            return total;
        } 

    }
}
