using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTOs;
using MediatR;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler: IRequestHandler<CreateOrderCommand, OrderDto>
    {
       
        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Here you would typically add logic to create the order in the database
            // For this example, we'll just return a new OrderDto with the provided data
            var order = new Order
            {  
                PromoCode = new PromoCode { Code = request.PromoCode },
                UserId = request.UserId,
                OrderItems = request.OrderItems,
                TotalAmount = CalculateTotalAmount(request.OrderItems)
            };
            await _repository.AddAsync(banner);
            await _unitOfWork.SaveChangesAsync();

            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Notes = order.Notes,
                UserName = order.UserName,
                Address = order.Address,
                PromoCode = order.PromoCode?.Code,
                OrderItems = order.OrderItems,
                Payment = order.Payment
            };
        }
        private decimal CalculateTotalAmount(List<OrderItemDto> orderItems)
        {
            // Simulate total amount calculation based on order items
            decimal total = 0;
            foreach (var item in orderItems)
            {
                total += item.Quantity * item.Price; // Assume each item has a Price property
            }
            return total;
        }

    }
}
