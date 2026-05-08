using ECommerce.Application.DTOs;
using ECommerce.Application.Features.CartItems.Commands.UpdateCartItem;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;


namespace ECommerce.Application.Features.Banners.Commands.UpdateBanner
{
    public class UpdateBannerCommandHandler:IRequestHandler<UpdateBannerCommand, BannerDto>
    {
        private readonly IBannerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBannerCommandHandler(IBannerRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BannerDto> Handle(UpdateBannerCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return null;
            item.Title = request.Title;
            item.ImageUrl = request.ImageUrl;
            item.Link = request.Link;
            item.IsActive = request.IsActive;
            item.DisplayOrder = request.DisplayOrder;
            await _repository.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();
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
}
