using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.Commands.UpdatePayment
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;

        // ضفنا الـ Repositories بتاعة الـ Product والـ Cart
        public ConfirmPaymentCommandHandler(
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<bool> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository
                .Table
                .Include(o => o.Payment)
                .Include(o => o.Shipments)
                .Include(o => o.OrderItems) // ضفنا دي
                .ThenInclude(oi => oi.Product) // وضفنا دي
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null || order.Payment == null)
                return false;

            if (order.Payment.Status == PaymentStatus.completed)
                return true;

            // 2. تحديث حالة الدفع والأوردر
            order.Payment.Status = PaymentStatus.completed;
            order.Payment.PaidAt = DateTime.UtcNow;
            order.Payment.TransactionId = request.TransactionId;

            order.Status = OrderStatus.Confirmed; 

            if (order.Shipments != null && order.Shipments.Any())
            {
                foreach (var shipment in order.Shipments)
                {
                    shipment.Status = ShipmentStatus.Processing;
                }
            }

            if (order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    if (item.Product != null)
                    {
                        item.Product.Stock -= item.Quantity;

                        if (item.Product.Stock < 0) item.Product.Stock = 0;

                        await _productRepository.UpdateAsync(item.Product);
                    }
                }
            }

            var carts = await _cartRepository.GetByConditionAsync(
                c => c.UserId == order.UserId && !c.IsDeleted,
                includeProperties: "CartItems"
            );
            var cart = carts.FirstOrDefault();

            if (cart != null && cart.CartItems != null)
            {
                foreach (var item in cart.CartItems)
                {
                    await _cartItemRepository.DeleteAsync(item.Id);
                }
            }

            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}