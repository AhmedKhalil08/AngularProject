using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Carts.Commands.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, bool>
    {
        private readonly ICartRepository _cartRepo;
        private readonly ICartItemRepository _cartItemRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ClearCartCommandHandler(
            ICartRepository cartRepo,
            ICartItemRepository cartItemRepo,
            IUnitOfWork unitOfWork)
        {
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var carts = await _cartRepo.GetAllAsync();
            var cart = carts.FirstOrDefault(c => c.UserId == request.UserId && !c.IsDeleted);

            if (cart == null) return false;

            var items = await _cartItemRepo.GetAllAsync();
            var cartItems = items.Where(i => i.CartId == cart.Id).ToList();

            foreach (var item in cartItems)
            {
                await _cartItemRepo.DeleteAsync(item.Id);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
