using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.PromoCodes.Commands.UpdatePromoCode
{
    public class UpdatePromoCodeCommand
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountPercent { get; set; }

        public int MaxUsageCount { get; set; }
    }
}
