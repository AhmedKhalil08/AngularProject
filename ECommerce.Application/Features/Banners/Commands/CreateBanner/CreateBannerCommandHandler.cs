using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Banners.Commands.CreateBanner
{
    public class CreateBannerCommandHandler : IRequestHandler<CreateBannerCommand, BannerDto>
    {
        private readonly IBannerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBannerCommandHandler(IBannerRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BannerDto> Handle(CreateBannerCommand request, CancellationToken cancellationToken)
        {

            var banner = new Banner
            {
                Title = request.Title,
                ImageUrl = request.ImageUrl,
                Link = request.Link,
                IsActive = request.IsActive,
                DisplayOrder = request.DisplayOrder
            };

            await _repository.AddAsync(banner);
            await _unitOfWork.SaveChangesAsync();

            return new BannerDto
            {
                Id = banner.Id,
                Title = banner.Title,
                ImageUrl = banner.ImageUrl,
                Link = banner.Link ?? string.Empty,
                IsActive = banner.IsActive,
                DisplayOrder = banner.DisplayOrder
            };
        }
    }
}
