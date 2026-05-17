using ECommerce.Application.DTOs;
using MediatR;
using System;

namespace ECommerce.Application.Features.PromoCodes.Commands.CreatePromoCode
{
    public class CreatePromoCodeCommand : IRequest<PromoCodeDto>
    {
        public string Code { get; set; }
        public decimal DiscountPercent { get; set; }
        public int MaxUsageCount { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
