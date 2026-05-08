using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entites;
using MediatR;

namespace ECommerce.Application.Features.Banners.Commands.CreateBanner
{
    internal class CreateBannerCommandHandler : IRequestHandler<CreateBannerCommand, BannerDto>
    {
        private readonly IBannerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBannerCommandHandler(IBannerRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {

            /*
              public string Title { get; set; } 
        public string ImageUrl { get; set; }
        public string? Link { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }*/
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
