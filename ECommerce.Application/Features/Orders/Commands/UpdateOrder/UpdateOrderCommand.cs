using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Enums;

namespace ECommerce.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<OrderDto>
    {
        public int Id { get; set; }

        // الحالة اللي الأدمن هيغيرها (مثلاً: Shipped, Delivered, Cancelled)
        public OrderStatus Status { get; set; }

        // ممكن نضيف ملاحظات لو الأدمن حابب يكتب سبب الإلغاء مثلاً
        public string? Notes { get; set; }
    }
}
