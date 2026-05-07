using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;

namespace ECommerce.Application.Features.CartItems.Commands.UpdateCartItem
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, CartItemDto>
    {
        private readonly ICartItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCartItemCommandHandler(ICartItemRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartItemDto> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return null;

            item.Quantity = request.Quantity;

            _repository.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return new CartItemDto
            {
                Id = item.Id,
                CartId = item.CartId,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            };
        }
    }
}
