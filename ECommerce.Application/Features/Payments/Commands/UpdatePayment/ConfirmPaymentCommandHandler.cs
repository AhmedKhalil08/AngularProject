using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Payments.Commands.UpdatePayment
{
    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmPaymentCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
            // 1. نجيب الأوردر ومعاه بيانات الـ Payment بتاعته
            var order = await _orderRepository.GetByIdAsync(request.OrderId);

            if (order == null || order.Payment == null)
                return false;

            // 2. تحديث حالة الدفع لـ "تمت بنجاح"
            order.Payment.Status = PaymentStatus.completed;
            order.Payment.PaidAt = DateTime.UtcNow; // 👈 هنا بقى نسجل وقت الدفع الحقيقي
            order.Payment.TransactionId = request.TransactionId;

            // 3. تحديث حالة الطلب نفسه لـ "جاري التجهيز" بما إن الفلوس وصلت
            order.Status = OrderStatus.Shipped;

            // 4. حفظ التعديلات في الداتابيز
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
