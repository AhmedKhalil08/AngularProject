using ECommerce.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.OrderItems.Commands.CreateOrderItem
{
    public class CreateOrderItemCommand: IRequest<OrderItemDto>
    {
      
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
