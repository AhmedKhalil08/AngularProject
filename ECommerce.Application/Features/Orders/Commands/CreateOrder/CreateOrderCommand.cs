using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<PaymentResultDto>
    {
        
        public List<CreateItemDto> Items { get; set; } = new();
        public string? PromoCode { get; set; }
        public PaymentMethod PaymentMethod { get; set; } 
        public AddressDto Address { get; set; } = new AddressDto();
    }
}
