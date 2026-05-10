using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Commands.UpdateSellerProfile
{
    public class UpdateSellerProfileCommandHandler : IRequestHandler<UpdateSellerProfileCommand, SellerProfileDto>
    {
        private readonly ISellerProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateSellerProfileCommandHandler(ISellerProfileRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currentUser=currentUser;
        }

        public async Task<SellerProfileDto> Handle(UpdateSellerProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByIdAsync(request.Id);
            if(profile==null) throw new NotFoundException("Seller profile not found");
            if (profile.UserId != _currentUser.UserId) throw new ForbiddenAccessException("this is not your profile");

            profile.StoreName = request.StoreName;
            profile.StoreDescription = request.StoreDescription;
            profile.LogoUrl = request.LogoUrl;

            await _repository.UpdateAsync(profile);
            await _unitOfWork.SaveChangesAsync();

            return new SellerProfileDto
            {
                Id = profile.Id,
                StoreName = profile.StoreName,
                StoreDescription = profile.StoreDescription,
                LogoUrl = profile.LogoUrl,
                IsApproved = profile.IsApproved,
                TotalEarnings = profile.TotalEarnings
            };
        }
    }
}
