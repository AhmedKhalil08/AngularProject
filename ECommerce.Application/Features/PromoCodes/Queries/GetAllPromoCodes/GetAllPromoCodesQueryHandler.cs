using System;
using System.Collections.Generic;
using System.Text;
using
    MediatR;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;

namespace ECommerce.Application.Features.PromoCodes.Queries.GetAllPromoCodes
{
    public class GetAllPromoCodesQueryHandler : IRequestHandler<GetAllPromoCodesQuery, List<PromoCodeDto>>
    {
        private readonly IPromoCodeRepository _repository;
        public GetAllPromoCodesQueryHandler(IPromoCodeRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<PromoCodeDto>> Handle(GetAllPromoCodesQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllAsync();
            return items.Select(p => new PromoCodeDto
            {
                Id = p.Id,
                Code = p.Code,
                DiscountPercent = p.DiscountPercent,
                MaxUsageCount = p.MaxUsageCount,
                CurrentUsageCount = p.CurrentUsageCount,
                ExpiryDate = p.ExpiryDate
            }).ToList();
        }
    }
}
