using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductImages.Commands.CreateProductImage
{
    public class CreateProductImageCommandHandler : IRequestHandler<CreateProductImageCommand, ProductImageDto>
    {
        private readonly IProductImageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductImageCommandHandler(IProductImageRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductImageDto> Handle(CreateProductImageCommand request, CancellationToken cancellationToken)
        {
            var image = new ProductImage
            {
                ProductId = request.ProductId,
                ImageUrl = request.ImageUrl,
                IsMain = request.IsMain
            };

            await _repository.AddAsync(image);
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
