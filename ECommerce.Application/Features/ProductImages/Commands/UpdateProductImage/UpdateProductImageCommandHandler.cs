using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductImages.Commands.UpdateProductImage
{
    public class UpdateProductImageCommandHandler : IRequestHandler<UpdateProductImageCommand, ProductImageDto>
    {
        private readonly IProductImageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductImageCommandHandler(IProductImageRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductImageDto> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _repository.GetByIdAsync(request.Id);
            if (image == null) return null;

            image.ImageUrl = request.ImageUrl;
            image.IsMain = request.IsMain;

            _repository.UpdateAsync(image);
            await _unitOfWork.SaveChangesAsync();

            return new ProductImageDto
            {
                Id = image.Id,
                ProductId = image.ProductId,
                ImageUrl = image.ImageUrl,
                IsMain = image.IsMain
            };
        }
    }
}
