using ECommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class PromoCode: BaseEntity<int>
    {
        //-------------------------------------------------------------------------
        //                             SELF PROPS
        //-------------------------------------------------------------------------
       
        public string Code { get; set; }
        public decimal DiscountPercent{ get; set; }
        public int MaxUsageCount { get; set; }
        public int CurrentUsageCount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive => ExpiryDate > DateTime.UtcNow;

        //-------------------------------------------------------------------------
        //                             FOREIGN KEY
        //-------------------------------------------------------------------------


        /*-------------------------------------------------------------------------*/
        //                          Navigation Props 
        /*-------------------------------------------------------------------------*/
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
