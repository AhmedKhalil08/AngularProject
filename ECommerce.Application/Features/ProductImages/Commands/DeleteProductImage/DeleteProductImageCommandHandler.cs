using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.ProductImages.Commands.DeleteProductImage
{
    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommand, bool>
    {
        private readonly IProductImageRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductImageCommandHandler(IProductImageRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteProductImageCommand request, CancellationToken cancellationToken)
        {
            var image = await _repository.GetByIdAsync(request.Id);
            if (image == null) return false;

            _repository.DeleteAsync(image.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
