using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class PromoCodeDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public decimal DiscountPercent { get; set; }

        public int MaxUsageCount { get; set; }

        public int CurrentUsageCount { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsActive => ExpiryDate > DateTime.UtcNow;
    }
}
