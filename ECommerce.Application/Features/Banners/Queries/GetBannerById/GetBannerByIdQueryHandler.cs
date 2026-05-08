using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

    public class GetBannerByIdQueryHandler : IRequestHandler<GetBannerByIdQuery, BannerDto>
    {
        private readonly IBannerRepository _repository;
        public GetBannerByIdQueryHandler(IBannerRepository repository)
        {
            _repository = repository;
        }
        public async Task<BannerDto> Handle(GetBannerByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return null;
            return new BannerDto
            {
                Id = item.Id,
                Title = item.Title,
                ImageUrl = item.ImageUrl,
                Link = item.Link,
                IsActive = item.IsActive,
                DisplayOrder = item.DisplayOrder
            };
        }
    }

