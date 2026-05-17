using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class ShipmentDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }
        public ShipmentStatus Status { get; set; } 
        public decimal ShippingFee { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();

    }
}
