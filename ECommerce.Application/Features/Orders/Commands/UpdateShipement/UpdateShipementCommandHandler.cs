using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Enums;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity; 
using ECommerce.Domain.Entities; 

namespace ECommerce.Application.Features.Orders.Commands.UpdateShipement
{
    public class UpdateShipementCommandHandler : IRequestHandler<UpdateShipementCommand, bool>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductRepository _productRepository;

        private readonly IMailConfService _mailConfService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateShipementCommandHandler(
            IShipmentRepository shipmentRepository,
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IProductRepository productRepository,
            IMailConfService mailConfService,
            UserManager<ApplicationUser> userManager)
        {
            _shipmentRepository = shipmentRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _productRepository = productRepository;

            // 👇 حقن السيرفيسز
            _mailConfService = mailConfService;
            _userManager = userManager;
        }

        public async Task<bool> Handle(UpdateShipementCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId; // ده الـ Seller
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Must be logged in.");

            var shipment = await _shipmentRepository.GetByIdAsync(request.ShipmentId);
            if (shipment == null) throw new Exception("Shipment not found.");

            var order = await _orderRepository.GetByIdAsync(shipment.OrderId);
            if (order == null) throw new Exception("Order not found.");

            if (shipment.SellerId != userId)
                throw new UnauthorizedAccessException("You can only update shipments for your own orders.");

            // إرجاع المخزون لو الشحنة اتلغت
            if (request.ShipmentStatus == ShipmentStatus.Cancelled && shipment.Status != ShipmentStatus.Cancelled)
            {
                foreach (var item in shipment.OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;
                        await _productRepository.UpdateAsync(product);
                    }
                }
            }

            shipment.Status = request.ShipmentStatus;
            await _shipmentRepository.UpdateAsync(shipment);

            var orderShipments = await _shipmentRepository.GetByOrderIdAsync(order.Id);

            bool hasShipments = orderShipments != null && orderShipments.Any();
            bool isAllDelivered = hasShipments && orderShipments.All(s => s.Status == ShipmentStatus.Delivered);
            bool isAllCancelled = hasShipments && orderShipments.All(s => s.Status == ShipmentStatus.Cancelled);
            bool isAllShipped = hasShipments && orderShipments.All(s =>
                s.Status == ShipmentStatus.Shipped ||
                s.Status == ShipmentStatus.Delivered);

            var originalOrderStatus = order.Status;

            if (isAllDelivered)
            {
                if (order.Status != OrderStatus.Delivered)
                {
                    order.Status = OrderStatus.Delivered;
                    await _orderRepository.UpdateAsync(order);
                }
            }
            else if (isAllShipped)
            {
                if (order.Status != OrderStatus.Shipped)
                {
                    order.Status = OrderStatus.Shipped;
                    await _orderRepository.UpdateAsync(order);
                }
            }
            else if (isAllCancelled)
            {
                if (order.Status != OrderStatus.Cancelled)
                {
                    order.Status = OrderStatus.Cancelled;
                    await _orderRepository.UpdateAsync(order);
                }
            }
            await _unitOfWork.SaveChangesAsync();
            if (originalOrderStatus != order.Status)
            {
                var customer = await _userManager.FindByIdAsync(order.UserId);

                if (customer != null && !string.IsNullOrEmpty(customer.Email))
                {
                    await _mailConfService.SendOrderStatusUpdateAsync(customer.Email, order.Id, order.Status.ToString());
                }
            }
            return true;
        }
    }
}