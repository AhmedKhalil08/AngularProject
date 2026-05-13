using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Carts.Commands.ClearCart
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, bool>
    {
        private readonly ICartRepository _cartRepo;
        private readonly ICartItemRepository _cartItemRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public ClearCartCommandHandler(
            ICartRepository cartRepo,
            ICartItemRepository cartItemRepo,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _cartRepo = cartRepo;
            _cartItemRepo = cartItemRepo;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Must be logged in.");

            // 1. استخدام GetByConditionAsync عشان نجيب سلة العميل ده بس ومعاها المنتجات بتاعتها (Single SQL Query)
            var carts = await _cartRepo.GetByConditionAsync(
                c => c.UserId == userId && !c.IsDeleted,
                includeProperties: "CartItems", // 👈 الكلمة السحرية عشان يجيب عناصر السلة معاه
                trackChanges: true
            );

            var cart = carts.FirstOrDefault();

            // لو مفيش سلة للعميل، أو السلة فاضية أصلاً، يبقى إحنا كده خلصنا بنجاح
            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                return true;

            // 2. نمسح المنتجات اللي جوه السلة دي بس
            foreach (var item in cart.CartItems.ToList())
            {
                await _cartItemRepo.DeleteAsync(item.Id);
            }

            // 3. نحفظ التغييرات في الداتابيز
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}