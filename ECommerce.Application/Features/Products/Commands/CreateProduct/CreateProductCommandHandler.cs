using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        private readonly ICurrentUserService _currentUserService;
        private readonly ISellerProfileRepository _sellerProfileRepository;

        public CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IFileService fileService, IProductImageRepository productImageRepository, ICurrentUserService currentUserService, ISellerProfileRepository sellerProfileRepository)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _productImageRepository = productImageRepository;
            _currentUserService = currentUserService;
            _sellerProfileRepository = sellerProfileRepository;
            
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var SellerId = _currentUserService.UserId;
            // 1. إنشاء أوبجكت المنتج (مع تجهيز لستة الصور)
            var approvedSeller = await _sellerProfileRepository.GetByUserIdAsync(SellerId);
            if (approvedSeller == null || !approvedSeller.IsApproved)
            {
                throw new Exception("Only approved sellers can create products.");
            }
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                Description = request.Description,
                CategoryId = request.CategoryId,
                SellerId=SellerId,
                Images = new List<ProductImage>() // 👈 تهيئة القائمة
            };

            // 2. معالجة الصور
            if (request.Images != null && request.Images.Any())
            {
                foreach (var image in request.Images)
                {
                    var imagePath = await _fileService.UploadFileAsync(image, "products");
                    // 👈 بنضيف الصورة للمنتج نفسه مش للـ Repository منفصل
                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = imagePath,
                        IsMain = (product.Images.Count == 0) // أول صورة تبقي هي الأساسية
                    });
                }
            }

            // 3. إضافة المنتج (الـ EF هيضيف معاه كل الصور اللي جوه الـ List بتاعته)
            await _productRepository.AddAsync(product);

            // 4. حفظ الكل في خبطة واحدة (Transaction واحدة)
            await _unitOfWork.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description,
                ImageUrls = product.Images.Select(i => i.ImageUrl).ToList()
                
            };
        }
    }
}
