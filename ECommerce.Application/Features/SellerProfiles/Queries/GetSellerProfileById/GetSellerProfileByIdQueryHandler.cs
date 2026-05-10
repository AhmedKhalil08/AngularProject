using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Queries.GetSellerProfileById
{
    public class GetSellerProfileByIdQueryHandler : IRequestHandler<GetSellerProfileByIdQuery, SellerProfileDto>
    {
        private readonly ISellerProfileRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public GetSellerProfileByIdQueryHandler(ISellerProfileRepository repository, ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser= currentUser;
        }

        public async Task<SellerProfileDto> Handle(GetSellerProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByIdAsync(request.Id);
            if (profile == null) throw new NotFoundException("Profile Not Found");
            if (profile.UserId != _currentUser.UserId) throw new ForbiddenAccessException("This is not your profile");

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
