using ECommerce.Application.Features.CartItems.Commands.UpdateCartItem;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Carts.Commands.UpdateCartItem
{
    public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, bool>
    {
        // استخدام الـ Generic Repository لجدول الـ CartItem
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCartItemCommandHandler(
            ICartItemRepository cartItemRepository,
            ICurrentUserService currentUser,
            IUnitOfWork unitOfWork)
        {
            _cartItemRepository = cartItemRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Must be logged in.");

            // 1. Fetch the CartItem, including Product (to check stock) and Cart (to verify ownership)
            var cartItems = await _cartItemRepository.GetByConditionAsync(
                ci => ci.Id == request.CartItemId,
                includeProperties: "Product,Cart",
                trackChanges: true // 👈 True because we want to update/delete it
            );

            var cartItem = cartItems.FirstOrDefault();

            if (cartItem == null)
                throw new Exception("Cart item not found.");

            // 2. Security Check: Ensure the user owns this cart item
            if (cartItem.Cart.UserId != userId)
                throw new UnauthorizedAccessException("You don't have permission to modify this cart.");

            // 3. Logic: If quantity is 0 or less, DELETE the item
            if (request.Quantity <= 0)
            {
                await _cartItemRepository.DeleteAsync(cartItem.Id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            // 4. Logic: If quantity > 0, check stock availability
            if (cartItem.Product.Stock < request.Quantity)
            {
                throw new Exception($"Cannot update quantity. Only {cartItem.Product.Stock} items available in stock.");
            }

            // 5. Update the quantity and Save
            cartItem.Quantity = request.Quantity;
            await _cartItemRepository.UpdateAsync(cartItem);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}