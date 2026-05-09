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
        private readonly IFileService _fileService; // 👈 ضفنا ده عشان الصور

        public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IFileService fileService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // 1. نجيب المنتج من الداتابيز ومعاه الصور الحالية بتاعته
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null) return null;

            // 2. تحديث البيانات الأساسية
            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;
            product.Description = request.Description;
            product.CategoryId = request.CategoryId;

            // 3. معالجة الصور الجديدة (لو اليوزر رفع صور جديدة في الـ Update)
            if (request.Images != null && request.Images.Any())
            {
                foreach (var image in request.Images)
                {
                    var imagePath = await _fileService.UploadFileAsync(image, "products");

                    // بنضيف الصورة الجديدة للـ Collection الحالية
                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = imagePath,
                        IsMain = false // ممكن تخلي اليوزر يحدد دي بعدين
                    });
                }
            }

            // 4. الحفظ
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
