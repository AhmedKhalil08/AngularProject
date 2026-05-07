using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Queries.GetAllSellerProfiles
{
    public class GetAllSellerProfilesQueryHandler : IRequestHandler<GetAllSellerProfilesQuery, List<SellerProfileDto>>
    {
        private readonly ISellerProfileRepository _repository;

        public GetAllSellerProfilesQueryHandler(ISellerProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SellerProfileDto>> Handle(GetAllSellerProfilesQuery request, CancellationToken cancellationToken)
        {
            var profiles = await _repository.GetAllAsync();

            return profiles
                .Where(p => !p.IsDeleted)
                .Select(p => new SellerProfileDto
                {
                    Id = p.Id,
                    StoreName = p.StoreName,
                    StoreDescription = p.StoreDescription,
                    LogoUrl = p.LogoUrl,
                    IsApproved = p.IsApproved,
                    TotalEarnings = p.TotalEarnings
                }).ToList();
        }
    }
}
