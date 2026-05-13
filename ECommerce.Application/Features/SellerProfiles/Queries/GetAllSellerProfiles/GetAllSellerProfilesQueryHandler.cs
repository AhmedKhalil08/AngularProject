using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.SellerProfiles.Queries.GetAllSellerProfiles
{
    public class GetAllSellerProfilesQueryHandler : IRequestHandler<GetAllSellerProfilesQuery, PagedResult<SellerProfileDto>>
    {
        private readonly ISellerProfileRepository _repository;

        public GetAllSellerProfilesQueryHandler(ISellerProfileRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResult<SellerProfileDto>> Handle(GetAllSellerProfilesQuery request, CancellationToken cancellationToken)
        {
            var profiles = await _repository.GetAllWithUserAsync();
            var query = profiles.AsQueryable();
            // search 
            if (!string.IsNullOrWhiteSpace(request.Search))
                query = query.Where(p => p.StoreName.Contains(request.Search) ||
                                         p.User.FullName.Contains(request.Search) ||
                                         p.User.Email.Contains(request.Search));

            // status
            query = request.Status switch
            {
                "approved" => query.Where(p => p.IsApproved && !p.IsDeleted && p.User.IsActive),
                "pending" => query.Where(p => !p.IsApproved && !p.IsDeleted && p.User.IsActive),
                "banned" => query.Where(p => !p.User.IsActive && !p.IsDeleted),
                "deleted" => query.Where(p => p.IsDeleted),
                _ => query
            };
            var totalCount = query.Count();

            var items = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new SellerProfileDto
            {
                Id = p.Id,
                StoreName = p.StoreName,
                StoreDescription = p.StoreDescription,
                LogoUrl = p.LogoUrl,
                IsApproved = p.IsApproved,
                TotalEarnings = p.TotalEarnings,
                IsDeleted = p.IsDeleted,
                UserId = p.UserId,
                FullName = p.User.FullName,
                Email = p.User.Email,
                IsActive = p.User.IsActive,
            }).ToList();



            return new PagedResult<SellerProfileDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
