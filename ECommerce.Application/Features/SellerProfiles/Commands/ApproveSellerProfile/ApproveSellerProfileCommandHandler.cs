using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace ECommerce.Application.Features.SellerProfiles.Commands.ApproveSellerProfile
{
    public class ApproveSellerProfileCommandHandler : IRequestHandler<ApproveSellerProfileCommand, SellerProfileDto>
    {
        private readonly ISellerProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ApproveSellerProfileCommandHandler(ISellerProfileRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SellerProfileDto> Handle(ApproveSellerProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByIdWithUserAsync(request.Id);

            if (profile == null)
                throw new NotFoundException("Seller profile not found");
            profile.IsApproved = request.IsApproved;

            await _repository.UpdateAsync(profile);
            await _unitOfWork.SaveChangesAsync();

            return new SellerProfileDto
            {
                Id = profile.Id,
                StoreName = profile.StoreName,
                StoreDescription = profile.StoreDescription,
                LogoUrl = profile.LogoUrl,
                IsApproved = profile.IsApproved,
                TotalEarnings = profile.TotalEarnings,
                UserId = profile.UserId,        
                FullName = profile.User.FullName, 
                Email = profile.User.Email
            };
        }
    }
}
