using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.CartItems.Commands.CreateCartItem
{
    public class CreateCartItemCommandHandler : IRequestHandler<CreateCartItemCommand, CartItemDto>
    {
        private readonly ICartItemRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCartItemCommandHandler(ICartItemRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartItemDto> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
        {
            var item = new CartItem
            {
                CartId = request.CartId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            await _repository.AddAsync(item);
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
