using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Payments.Commands.UpdatePayment;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.PromoCodes.Commands.UpdatePromoCode
{
    public class UpdatePromoCodeCommandHandler : IRequestHandler<UpdatePromoCodeCommand, PromoCodeDto>
    {
        private readonly IPromoCodeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePromoCodeCommandHandler(IPromoCodeRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<PromoCodeDto> Handle(UpdatePromoCodeCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return null;

            if (item == null)
                throw new Exception("Payment not found");

           item.Code = request.Code;
            item.DiscountPercent = request.DiscountPercent;


            await _unitOfWork.SaveChangesAsync(cancellationToken);
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
