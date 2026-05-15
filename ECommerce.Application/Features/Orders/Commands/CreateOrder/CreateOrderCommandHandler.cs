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
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICurrentUserService currentUserService,
            IPaymentService paymentService,
            IUnitOfWork unitOfWork,
            IShipmentRepository shipmentRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _paymentService = paymentService;
            _shipmentRepository = shipmentRepository;
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<PaymentResultDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Must be logged in.");

            var carts = await _cartRepository.GetByConditionAsync(
                c => c.UserId == userId && !c.IsDeleted,
                includeProperties: "CartItems,CartItems.Product"
            );
            var cart = carts.FirstOrDefault();

            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
                throw new Exception("Your cart is empty.");

            // 1. هل الدفع كاش؟
            bool isCashOnDelivery = request.PaymentMethod == PaymentMethod.CashOnDelivery;

            var itemsBySeller = new Dictionary<string, List<OrderItem>>();
            var allOrderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var cartItem in cart.CartItems)
            {
                var product = cartItem.Product;

                if (product.Stock == 0) continue;
                if (product.Stock < cartItem.Quantity) throw new Exception($"The stock of {product.Name} is insufficient.");

                totalAmount += product.Price * cartItem.Quantity;

                // 2. خصم المخزون فوراً "فقط" لو الدفع كاش
                if (isCashOnDelivery)
                {
                    product.Stock -= cartItem.Quantity;
                    await _productRepository.UpdateAsync(product);
                }

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price
                };

                if (!itemsBySeller.ContainsKey(product.SellerId))
                {
                    itemsBySeller[product.SellerId] = new List<OrderItem>();
                }
                itemsBySeller[product.SellerId].Add(orderItem);
                allOrderItems.Add(orderItem);
            }

            var shipments = new List<Shipment>();
            var shipingFee = 50;
            var orderTotalAmount = totalAmount + (itemsBySeller.Count * shipingFee);

            foreach (var sellerGroup in itemsBySeller)
            {
                decimal totalItemsAmount = sellerGroup.Value.Sum(oi => oi.UnitPrice * oi.Quantity);
                var shipment = new Shipment
                {
                    SellerId = sellerGroup.Key,
                    Status = isCashOnDelivery ? ShipmentStatus.Processing : ShipmentStatus.Pending,
                    ShippingFee = shipingFee,
                    TotalAmount = totalItemsAmount + shipingFee,
                    OrderItems = sellerGroup.Value
                };
                shipments.Add(shipment);
            }

            var shippingAddress = request.Address.Adapt<Address>();
            shippingAddress.UserId = userId;

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = orderTotalAmount,
                Status = isCashOnDelivery ? OrderStatus.Confirmed : OrderStatus.Pending,
                PaymentMethod = request.PaymentMethod,
                ShippingAddress = shippingAddress,
                OrderItems = allOrderItems,
                Shipments = shipments,
                Payment = new Payment
                {
                    Amount = orderTotalAmount,
                    Method = request.PaymentMethod,
                    Status = PaymentStatus.Pending
                }
            };

            await _orderRepository.AddAsync(order);

            if (isCashOnDelivery)
            {
                foreach (var item in cart.CartItems)
                {
                    await _cartItemRepository.DeleteAsync(item.Id);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            if (isCashOnDelivery)
            {
                return new PaymentResultDto
                {
                    IsSuccess = true,
                    PaymentUrl = null,
                    Message = "Order confirmed. Payment will be collected upon delivery."
                };
            }
            else
            {
                var paymentResult = await _paymentService.ProcessPaymentAsync(orderTotalAmount, request.PaymentMethod, order.Id);
                return paymentResult;
            }
        }
    }
}