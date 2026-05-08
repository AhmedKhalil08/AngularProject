using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public string? Notes { get; set; }

        public string UserName { get; set; }

        public string Address { get; set; }

        public string? PromoCode { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
        public PaymentDto? Payment { get; set; }
    }
}
