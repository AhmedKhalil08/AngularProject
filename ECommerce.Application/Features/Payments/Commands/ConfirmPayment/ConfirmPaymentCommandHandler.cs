using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.Commands.UpdatePayment
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmPaymentCommandHandler(IOrderRepository orderRepository,IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            // 1. نجيب الأوردر مع الـ Payment باستخدام الـ Table والـ Include
            var order = await _orderRepository
                .Table
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null || order.Payment == null)
                return false;

            // 💡 2. الحماية من تكرار الـ Webhook (Idempotency)
            if (order.Payment.Status == PaymentStatus.completed)
                return true; // نرجع true عشان بوابة الدفع تسكت ومتبعتش تاني

            // 3. تحديث حالة الدفع
            order.Payment.Status = PaymentStatus.completed;
            order.Payment.PaidAt = DateTime.UtcNow;
            order.Payment.TransactionId = request.TransactionId;

            // 4. تحديث حالة الطلب لـ "جاري التجهيز" (وليس تم الشحن)
            order.Status = OrderStatus.Shipped;

            // 5. حفظ التعديلات في الداتابيز
            // بنستخدم Repository الـ Order من الـ UnitOfWork عشان الـ Update
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}