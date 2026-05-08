using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;

namespace ECommerce.Application.Features.PromoCodes.Queries.GetPromoCodeById
{
    public class GetPromoCodeByIdQueryHandler : IRequestHandler<GetPromoCodeByIdQuery, PromoCodeDto>
    {
        private readonly IPromoCodeRepository _repository;
        public GetPromoCodeByIdQueryHandler(IPromoCodeRepository repository)
        {
            _repository = repository;
        }
        public async Task<PromoCodeDto> Handle(GetPromoCodeByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return null;

            return new PromoCodeDto
            {
                Id = item.Id,
                Code = item.Code,
                DiscountPercent = item.DiscountPercent,
                MaxUsageCount = item.MaxUsageCount,
                CurrentUsageCount = item.CurrentUsageCount,
                ExpiryDate = item.ExpiryDate
            };
        }
    }
}