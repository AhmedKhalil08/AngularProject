using ECommerce.Application.DTOs;
using ECommerce.Application.Features.Banners.Queries.GetAllBanners;
using ECommerce.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;


namespace ECommerce.Application.Features.Banners.Queries.GetAllBanners
{
    public class GetAllBannersQueryHandler : IRequestHandler<GetAllBannersQuery, List<BannerDto>>
    {
        private readonly IBannerRepository _repository;

        public GetAllBannersQueryHandler(IBannerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BannerDto>> Handle(GetAllBannersQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllAsync();
            return items.Select(i => new BannerDto
            {
                Id = i.Id,
                Title = i.Title,
                ImageUrl = i.ImageUrl,
                Link = i.Link,
                IsActive = i.IsActive,
                DisplayOrder = i.DisplayOrder
            }).ToList();

        }
    }
}
