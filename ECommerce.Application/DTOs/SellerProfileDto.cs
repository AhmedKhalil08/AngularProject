using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class SellerProfileDto
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string? StoreDescription { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsApproved { get; set; }
        public decimal TotalEarnings { get; set; }
    }
}
