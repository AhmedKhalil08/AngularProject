using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.Features.Carts.Commands.AddToCart
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, bool>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository; // عشان نتأكد من المخزن
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public AddToCartCommandHandler(
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IProductRepository productRepository,
            ICurrentUserService currentUser,
            IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Must be logged in.");
            //if (_currentUser.Role == "Admin" || _currentUser.Role == "Seller")
            //{
            //    throw new UnauthorizedAccessException("Cart Can't be for  seller or Admin");
            //}

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null) throw new Exception("Product not found.");
            if (product.Stock < request.Quantity) throw new Exception("Not enough stock.");

            var carts = await _cartRepository.GetByConditionAsync(
                c => c.UserId == userId && !c.IsDeleted,
                includeProperties: "CartItems",
                trackChanges: true
            );
            var cart = carts.FirstOrDefault();

          
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    IsDeleted = false
                };
                await _cartRepository.AddAsync(cart);

                await _unitOfWork.SaveChangesAsync();
            }

            var existingCartItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == request.ProductId);

            if (existingCartItem != null)
            {
                if (existingCartItem.Quantity + request.Quantity > product.Stock)
                    throw new Exception("Exceeds available stock.");

                existingCartItem.Quantity += request.Quantity;
                await _cartItemRepository.UpdateAsync(existingCartItem);
            }
            else
            {
                var newCartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                await _cartItemRepository.AddAsync(newCartItem);
            }

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}

