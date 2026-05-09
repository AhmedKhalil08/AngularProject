using ECommerce.Domain.Enums;
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

        public AddressDto Address { get; set; }

        public PromoCodeDto? PromoCode { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }=new List<OrderItemDto>();
        public PaymentDto Payment { get; set; }
    }
}
