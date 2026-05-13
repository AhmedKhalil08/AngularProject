using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.CartItems.Commands.DeleteCartItem
{
    public class DeleteCartItemCommandHandler : IRequestHandler<DeleteCartItemCommand, bool>
    {
        private readonly ICartItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCartItemCommandHandler(ICartItemRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.CartItemId);
            if (item == null) return false;

            await _repository.DeleteAsync(item.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
