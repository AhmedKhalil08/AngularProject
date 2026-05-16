using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly ICurrentUserService _currentUserService;

        public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IFileService fileService, ICurrentUserService currentUserService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_currentUserService.UserId))
                throw new UnauthorizedAccessException("Must be logged in.");
            if (!await _productRepository.IsUserOwnerOfProductAsync(request.Id, _currentUserService.UserId)&&(_currentUserService.Role!="Admin"))
                throw new UnauthorizedAccessException("You can only delete your own products.");

            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null) return false;

            if (product.Images != null)
            {
                foreach (var img in product.Images)
                {
                    _fileService.DeleteFile(img.ImageUrl); 
                }
            }

            await _productRepository.DeleteAsync(product.Id);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
