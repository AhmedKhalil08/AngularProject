using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Mapster;
using MediatR;

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, PaymentResultDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPaymentService _paymentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShipmentRepository _shipmentRepository;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICurrentUserService currentUserService,
            IPaymentService paymentService,
            IUnitOfWork unitOfWork,
            IShipmentRepository shipmentRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _paymentService = paymentService;
            _shipmentRepository = shipmentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentResultDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var itemsBySeller = new Dictionary<string, List<OrderItem>>();
            var allOrderItems = new List<OrderItem>();
            //var userId = _currentUserService.UserId;
            //if (string.IsNullOrEmpty(userId))
            //    throw new UnauthorizedAccessException("Must be logged in.");
            var userId = "620309fb-6d65-4fb5-ba45-14fb66df6bf9";
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var item in request.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) throw new Exception($"Product with ID {item.ProductId} not found.");
                if (product.Stock == 0) continue;
                if (product.Stock < item.Quantity) throw new Exception($"The stock of {product.Name} is insufficient.");

                totalAmount += product.Price * item.Quantity;
                product.Stock -= item.Quantity;
                await _productRepository.UpdateAsync(product);

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                // 2. تصنيف الـ OrderItem ووضعه في القائمة الخاصة بالبائع بتاعه
                if (!itemsBySeller.ContainsKey(product.SellerId))
                {
                    itemsBySeller[product.SellerId] = new List<OrderItem>();
                }
                itemsBySeller[product.SellerId].Add(orderItem);
                allOrderItems.Add(orderItem); // إضافته للقائمة الكلية أيضاً
            }

            // 3. تحويل التجميعة إلى شحنات (Shipments) حقيقية
            var shipments = new List<Shipment>();
            var shipingFee = 50; // ثابت لكل شحنة
            var orderTotalAmount = totalAmount + (itemsBySeller.Count * shipingFee); // إجمالي الأوردر = مجموع المنتجات + مجموع الشحنات
            foreach (var sellerGroup in itemsBySeller)
            {
                decimal totalItemsAmount = sellerGroup.Value.Sum(oi => oi.UnitPrice * oi.Quantity);
                var shipment = new Shipment
                {
                    SellerId = sellerGroup.Key,
                    Status = ShipmentStatus.Pending,
                    ShippingFee = shipingFee,
                    TotalAmount = totalItemsAmount + shipingFee,
                    OrderItems = sellerGroup.Value ,
                    
                    
                };
                shipments.Add(shipment);
            }

            // PromoCode (إذا وجد)
            var shippingAddress = request.Address.Adapt<Address>();
            shippingAddress.UserId = userId; 
            // 4. بناء الأوردر الأساسي
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = orderTotalAmount,
                Status = OrderStatus.Pending,
                PaymentMethod = request.PaymentMethod,
                

                ShippingAddress = shippingAddress,
                
                OrderItems = allOrderItems, 
                Shipments = shipments,      

                Payment = new Payment
                {
                    Amount = totalAmount,
                    Method = request.PaymentMethod,
                    Status = PaymentStatus.Pending
                }
            };

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync(); // الـ EF Core هيربط الـ Ids تلقائياً

            // 6. معالجة الدفع
            var paymentResult = await _paymentService.ProcessPaymentAsync(orderTotalAmount, request.PaymentMethod, order.Id);

            return paymentResult;
        }
    }
}
