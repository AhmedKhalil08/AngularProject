using ECommerce.Application.DTOs;
using ECommerce.Application.Features.OrderItems.Commands.UpdateOrderItem;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. البحث عن الطلب في الداتابيز
            var order = await _orderRepository.GetByIdAsync(request.Id);

            if (order == null)
            {
                throw new Exception($"الطلب رقم {request.Id} غير موجود.");
            }

            // 2. تحديث البيانات المسموح بتحديثها فقط
            order.Status = request.Status;

            if (!string.IsNullOrEmpty(request.Notes))
            {
                order.Notes = request.Notes;
            }

            // 3. تحديث في الميموري ثم حفظ في قاعدة البيانات
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            // 4. إرجاع الـ DTO (Mapping)
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount, // بنرجع الإجمالي الأصلي اللي متسجل في الداتابيز
                Status = order.Status,
                Notes = order.Notes,

                // لو عامل Include للـ User والـ Items في الـ Repository، تقدر ترجعهم هنا كمان
                // UserName = order.User?.UserName,
                // إلخ...
            };
        }
    }
}
