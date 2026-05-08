using System.Threading;
using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.PromoCodes.Commands.CreatePromoCode
{
    public class CreatePromoCodeCommandHandler : IRequestHandler<CreatePromoCodeCommand, PromoCodeDto>
    {
        private readonly IPromoCodeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePromoCodeCommandHandler(IPromoCodeRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PromoCodeDto> Handle(CreatePromoCodeCommand request, CancellationToken cancellationToken)
        {
            var promo = new PromoCode
            {
                Code = request.Code,
                DiscountPercent = request.DiscountPercent,
                MaxUsageCount = request.MaxUsageCount,
                CurrentUsageCount = 0,
                ExpiryDate = request.ExpiryDate
            };

            await _repository.AddAsync(promo);
            await _unitOfWork.SaveChangesAsync();

            return new PromoCodeDto
            {
                Id = promo.Id,
                Code = promo.Code,
                DiscountPercent = promo.DiscountPercent,
                MaxUsageCount = promo.MaxUsageCount,
                CurrentUsageCount = promo.CurrentUsageCount,
                ExpiryDate = promo.ExpiryDate
            };
        }
    }
}
