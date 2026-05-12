using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class OrderItem: BaseEntity<int>
    {
        //-------------------------------------------------------------------------
        //                             SELF PROPS
        //-------------------------------------------------------------------------
      
        public int Quantity { get; set; }
         public decimal UnitPrice { get; set; }
        //-------------------------------------------------------------------------
        //                             FOREIGN KEY
        //-------------------------------------------------------------------------
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? ShipmentId { get; set; } 
        /*-------------------------------------------------------------------------*/
        //                          Navigation Props 
        /*-------------------------------------------------------------------------*/
        public Order Order { get; set; }
        public Product Product { get; set; }
        public Shipment Shipment { get; set; }
    }
}
