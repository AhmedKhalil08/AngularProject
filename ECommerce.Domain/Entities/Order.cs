using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;
namespace ECommerce.Domain.Entities
{
    public class Order : BaseEntity<int>
    {
        //-------------------------------------------------------------------------
        //                             SELF PROPS
        //-------------------------------------------------------------------------
       
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string? Notes { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        //-------------------------------------------------------------------------
        //                             FOREIGN KEY
        //-------------------------------------------------------------------------
        public string UserId { get; set; }
        public int ShippingAddressId { get; set; }
        public int? PromoCodeId { get; set; }
        /*-------------------------------------------------------------------------*/
        //                          Navigation Props 
        /*-------------------------------------------------------------------------*/
        public ApplicationUser User { get; set; }
        public Address ShippingAddress { get; set; }
        public PromoCode? PromoCode { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }=new List<OrderItem>();
        public Payment? Payment { get; set; }
    }
}
