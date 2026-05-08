using ECommerce.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand:IRequest<OrderDto>
    {
       
        public int UserId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }

        public string? PromoCode { get; set; }

        public PaymentDto Payment { get; set; }
    }
}
