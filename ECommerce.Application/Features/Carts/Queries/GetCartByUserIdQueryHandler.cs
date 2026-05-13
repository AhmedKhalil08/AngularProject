using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Carts.Queries
{
    public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, CartDto>
    {
        private readonly ICartRepository _repository;
        private readonly ICurrentUserService _currentUser;

        public GetCartByUserIdQueryHandler(ICartRepository repository, ICurrentUserService currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<CartDto> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            //var userId = _currentUser.UserId;
            //if (string.IsNullOrEmpty(userId))
            //throw new UnauthorizedAccessException("Must be logged in.");
            var userId = "c43c69b5-9fd5-40d1-b91b-caa888ed98e8";
            var carts = await _repository.GetByConditionAsync(
        c => c.UserId == userId && !c.IsDeleted,
        // 👈 زودنا Images هنا عشان نقدر نوصل لصورة المنتج
        includeProperties: "CartItems,CartItems.Product,CartItems.Product.Images",
        trackChanges: false
    );
            // No need to repeat the condition here, the data is already filtered
            var cart = carts.FirstOrDefault();
            if (cart == null)
                return null;
            return new CartDto
            {
                Id = cart.Id,
                Items = cart.CartItems?.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? "Unknown",
                    Quantity = i.Quantity,
                    UnitPrice = i.Product?.Price ?? 0,
                    // 👈 بنجيب الصورة اللي معلم عليها IsMain أو أول صورة موجودة
                    ProductImage = i.Product?.Images?.FirstOrDefault(img => img.IsMain)?.ImageUrl
                                   ?? i.Product?.Images?.FirstOrDefault()?.ImageUrl
                                   ?? "default-product.png",
                    StockQuantity = i.Product?.Stock ?? 0
                }).ToList() ?? new List<CartItemDto>(),

                TotalPrice = cart.CartItems?.Sum(i => i.Quantity * (i.Product?.Price ?? 0)) ?? 0
            };
        }
    }
}