using ECommerce.Application.Features.Banners.Commands.DeleteBanner;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.PromoCodes.Commands.DeletePromoCode
{
    public class DeletePromoCodeHandler
    {
        public class DeletePromoCodeCommandHandler : IRequestHandler<DeletePromoCodeCommand, bool>
        {
            private readonly IPromoCodeRepository _repository;
            private readonly IUnitOfWork _unitOfWork;

            public DeletePromoCodeCommandHandler(IPromoCodeRepository repository, IUnitOfWork unitOfWork)
            {
                _repository = repository;
                _unitOfWork = unitOfWork;
            }

            public async Task<bool> Handle(DeletePromoCodeCommand request, CancellationToken cancellationToken)
            {
                var item = await _repository.GetByIdAsync(request.Id);
                if (item == null) return false;

                await _repository.DeleteAsync(item.Id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }
    }
}
