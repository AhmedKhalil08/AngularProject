using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, PaymentResultDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository; // عشان نجيب السعر الحقيقي
        //private readonly ICurrentUserService _currentUserService; // عشان نجيب اليوزر
        private readonly IPaymentService _paymentService; // خدمة الدفع
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            //ICurrentUserService currentUserService,
            IPaymentService paymentService,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            //_currentUserService = currentUserService;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentResultDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. جلب User ID أمنياً من التوكن
            //var userId = _currentUserService.UserId;
            //if (string.IsNullOrEmpty(userId))
            //    throw new UnauthorizedAccessException("يجب تسجيل الدخول أولاً.");

            // 2. التحقق من الأسعار الحقيقية من الداتابيز وحساب الإجمالي
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) throw new Exception($"المنتج رقم {item.ProductId} غير موجود.");
                if (product.Stock < item.Quantity) throw new Exception($"الكمية المطلوبة من {product.Name} غير متوفرة.");

                totalAmount += product.Price * item.Quantity; // السعر من الداتابيز مش من الريكويست!

                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price 
                });
            }

            // (هنا ممكن تضيف لوجيك الخصم بتاع الـ PromoCode لو موجود)

            // 3. إنشاء الـ Order Entity بدون ما نكريت Entities تانية جواها
            var order = new Order
            {
                //UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = OrderStatus.Pending,
                PaymentMethod = request.PaymentMethod,
                OrderItems = orderItems,
                ShippingAddress = new Address
                {
                    FullName = request.Address.FullName,
                    Street = request.Address.Street,
                    City = request.Address.City,
                    State = request.Address.State,
                    Country = request.Address.Country,
                    ZipCode = request.Address.ZipCode,
                    Phone = request.Address.Phone
                },
                Payment = new Payment
                {
                    Amount = totalAmount,
                    Method = request.PaymentMethod,
                    Status = PaymentStatus.Pending
                }
            };

            await _orderRepository.AddAsync(order);
            // لازم نعمل SaveChanges عشان الـ Order ياخد Id في الداتابيز
            await _unitOfWork.SaveChangesAsync();

            // 4. تشغيل خدمة الدفع (Stripe أو PayPal) بناءً على الإجمالي والنوع
            var paymentResult = await _paymentService.ProcessPaymentAsync(totalAmount, request.PaymentMethod, order.Id  );

            // لو حابين نحفظ الـ TransactionId اللي راجع من الدفع
            // order.Payment.TransactionId = paymentResult.TransactionId;
            // await _unitOfWork.SaveChangesAsync();

            return paymentResult; // بنرجع لينك الدفع للـ Front-end
        }
    }
}
