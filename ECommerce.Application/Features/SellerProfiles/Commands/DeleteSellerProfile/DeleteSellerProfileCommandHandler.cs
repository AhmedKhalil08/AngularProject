using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
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
        private readonly ICurrentUserService _currentUser;

        public DeleteSellerProfileCommandHandler(ISellerProfileRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentUser=currentUser;
        }

        public async Task<bool> Handle(DeleteSellerProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByIdAsync(request.Id);
            if (profile == null) throw new NotFoundException("Seller Profile Not Found");
            if (profile.UserId != _currentUser.UserId) throw new ForbiddenAccessException("this is not your profile");

            profile.IsDeleted = true;

            await _repository.UpdateAsync(profile);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

