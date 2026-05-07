using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Commands.CreateSellerProfile
{
    public class CreateSellerProfileCommandHandler : IRequestHandler<CreateSellerProfileCommand, SellerProfileDto>
    {
        private readonly ISellerProfileRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSellerProfileCommandHandler(ISellerProfileRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SellerProfileDto> Handle(CreateSellerProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = new SellerProfile
            {
                UserId = request.UserId,
                StoreName = request.StoreName,
                StoreDescription = request.StoreDescription,
                LogoUrl = request.LogoUrl,
                IsApproved = false,
                TotalEarnings = 0,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(profile);
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
