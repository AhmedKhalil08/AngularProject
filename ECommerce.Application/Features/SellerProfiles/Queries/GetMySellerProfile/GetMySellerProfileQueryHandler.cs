using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;

namespace ECommerce.Application.Features.SellerProfiles.Queries.GetMySellerProfile
{
    public class GetMySellerProfileQueryHandler : IRequestHandler<GetMySellerProfileQuery, SellerProfileDto>
    {
        private readonly ISellerProfileRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public GetMySellerProfileQueryHandler(ISellerProfileRepository repository, ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<SellerProfileDto> Handle(GetMySellerProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByUserIdAsync(_currentUser.UserId);
            if (profile == null) throw new NotFoundException("Seller profile not found");

            return new SellerProfileDto
            {
                Id = profile.Id,
                StoreName = profile.StoreName,
                StoreDescription = profile.StoreDescription,
                LogoUrl = profile.LogoUrl,
                IsApproved = profile.IsApproved,
                TotalEarnings = profile.TotalEarnings,
                UserId = profile.UserId
            };
        }
    }
}