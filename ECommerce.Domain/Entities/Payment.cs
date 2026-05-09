using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Domain.Common;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities
{
    public class Payment: BaseEntity<int>
    {
        //-------------------------------------------------------------------------
        //                             SELF PROPS
        //-------------------------------------------------------------------------

        public decimal Amount { get; set; }
        public string? TransactionId { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? PaidAt { get; set; }
        //-------------------------------------------------------------------------
        //                             FOREIGN KEY
        //-------------------------------------------------------------------------
        public int OrderId { get; set; }
        /*-------------------------------------------------------------------------*/
        //                          Navigation Props 
        /*-------------------------------------------------------------------------*/
         public Order Order { get; set; }
    }
}
