using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand:IRequest<OrderDto>
    {
       
        public string Id { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        public string? PromoCode { get; set; }

        public PaymentDto Payment { get; set; }

        public AddressDto Address { get; set; } = new AddressDto();
    }
}
