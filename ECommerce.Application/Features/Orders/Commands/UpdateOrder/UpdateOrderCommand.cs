using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand: IRequest<OrderDto>
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int TotalAmount { get; set; }

        public PaymentDto Payment { get; set; }
    }
}
