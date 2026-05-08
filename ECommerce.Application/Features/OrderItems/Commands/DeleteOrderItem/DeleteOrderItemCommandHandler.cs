using ECommerce.Application.Interfaces.Persistence;
using MediatR;

namespace ECommerce.Application.Features.OrderItems.Commands.DeleteOrderItem
{
    public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, bool>
    {
        private readonly IOrderItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderItemCommandHandler(IOrderItemRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return false;

            await _repository.DeleteAsync(item.Id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
