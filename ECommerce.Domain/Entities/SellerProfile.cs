using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.Entities
{
    public class SellerProfile : BaseEntity<int>
    {
        //-------------------------------------------------------------------------
        //                             SELF PROPS
        //-------------------------------------------------------------------------
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string? StoreDescription { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsApproved { get; set; }
        public decimal TotalEarnings { get; set; }
        public DateTime CreatedAt { get; set; }
        //-------------------------------------------------------------------------
        //                             FOREIGN KEY
        //-------------------------------------------------------------------------
        public string UserId { get; set; }
        //-------------------------------------------------------------------------
        //                             NAVIGATION
        //-------------------------------------------------------------------------
        public ApplicationUser User { get; set; }
    }
}
