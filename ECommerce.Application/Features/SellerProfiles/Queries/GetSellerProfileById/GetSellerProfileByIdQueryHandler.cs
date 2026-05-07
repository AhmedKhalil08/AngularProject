using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Queries.GetSellerProfileById
{
    public class GetSellerProfileByIdQueryHandler : IRequestHandler<GetSellerProfileByIdQuery, SellerProfileDto>
    {
        private readonly ISellerProfileRepository _repository;

        public GetSellerProfileByIdQueryHandler(ISellerProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<SellerProfileDto> Handle(GetSellerProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var profile = await _repository.GetByIdAsync(request.Id);

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
