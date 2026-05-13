using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Carts.Commands.SyncCart
{
    public class SyncCartCommandHandler: IRequestHandler<SyncCartCommand, bool>
    {
        private readonly ICartRepository _cartRepo;
        private readonly ICartItemRepository _cartItemRepo;
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;

        public SyncCartCommandHandler(ICartRepository cartRepo, ICartItemRepository cartItemRepo, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(SyncCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException();

            // 1. Get or Create Cart
            var carts = await _cartRepo.GetByConditionAsync(c => c.UserId == userId && !c.IsDeleted, includeProperties: "CartItems", trackChanges: true);
            var cart = carts.FirstOrDefault() ?? new Cart { UserId = userId};

            if (cart.Id == 0) await _cartRepo.AddAsync(cart);
            await _unitOfWork.SaveChangesAsync(); // عشان نضمن وجود ID للسلة

            // 2. Merge Logic
            foreach (var syncItem in request.Items)
            {
                var existingItem = cart.CartItems?.FirstOrDefault(ci => ci.ProductId == syncItem.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += syncItem.Quantity; // تحديث الكمية لو المنتج موجود
                }
                else
                {
                    await _cartItemRepo.AddAsync(new CartItem { CartId = cart.Id, ProductId = syncItem.ProductId, Quantity = syncItem.Quantity });
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
