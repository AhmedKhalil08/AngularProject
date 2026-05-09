using ECommerce.Application.Features.OrderItems.Commands.DeleteOrderItem;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository; // 👈 هنحتاجه عشان نعدل الـ Stock
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. لازم نتأكد إننا بنجيب الأوردر ومعاه الـ Items بتاعته
            // (تأكد إن ريبوزيتوري الـ Order بيعمل Include للـ OrderItems هنا)
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null) return false;

            // 💡 حماية إضافية: نمنع مسح أوردر مدفوع!
            if (order.Payment != null && order.Payment.Status == Domain.Enums.PaymentStatus.completed)
            {
                throw new Exception("لا يمكن مسح طلب تم دفعه بالفعل. يجب عمل استرجاع (Refund) أولاً.");
            }

            // 2. إرجاع المخزون (Restock)
            if (order.OrderItems != null && order.OrderItems.Any())
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Stock += item.Quantity; 
                        await _productRepository.UpdateAsync(product);
                    }
                }
            }

            // 3. مسح الأوردر 
            await _orderRepository.DeleteAsync(order.Id);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}