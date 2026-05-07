using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Commands.DeleteSellerProfile
{
    public class DeleteSellerProfileCommandHandler : IRequestHandler<DeleteSellerProfileCommand, bool>
    {
        private readonly ISellerProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSellerProfileCommandHandler(ISellerProfileRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteSellerProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByIdAsync(request.Id);

            profile.IsDeleted = true;

            await _repository.UpdateAsync(profile);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

