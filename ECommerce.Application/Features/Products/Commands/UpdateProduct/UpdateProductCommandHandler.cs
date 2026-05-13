using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IFileService fileService, ICurrentUserService currentUserService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _currentUserService = currentUserService;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_currentUserService.UserId))
                throw new UnauthorizedAccessException("Must be logged in.");
            if (!await _productRepository.IsUserOwnerOfProductAsync(request.Id, _currentUserService.UserId) && (_currentUserService.Role != "Admin"))
                throw new UnauthorizedAccessException("You can only update your own products.");

            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null) return null;

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;
            product.Description = request.Description;
            product.CategoryId = request.CategoryId;

            if (request.Images != null && request.Images.Any())
            {
                foreach (var image in request.Images)
                {
                    var imagePath = await _fileService.UploadFileAsync(image, "products");

                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = imagePath,
                        IsMain = false 
                    });
                }
            }

            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description
            };
        }
    }
}
