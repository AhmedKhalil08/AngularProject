using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class Shipment :AuditableEntity<int>
    {
        public int OrderId { get; set; }

        // The seller responsible for this specific shipment
        public string SellerId { get; set; }

        public ShipmentStatus Status { get; set; } = ShipmentStatus.Pending;

        public string? TrackingNumber { get; set; }
        public decimal ShippingFee { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public SellerProfile Seller { get; set; }

        // The items from the main order that belong ONLY to this seller
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
