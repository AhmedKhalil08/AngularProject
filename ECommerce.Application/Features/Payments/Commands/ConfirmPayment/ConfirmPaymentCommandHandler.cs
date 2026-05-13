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
            var order = await _orderRepository
                .Table
                .Include(o => o.Payment)
                .Include(o => o.Shipments) 
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

            if (order == null || order.Payment == null)
                return false;

            if (order.Payment.Status == PaymentStatus.completed)
                return true;

            order.Payment.Status = PaymentStatus.completed;
            order.Payment.PaidAt = DateTime.UtcNow;
            order.Payment.TransactionId = request.TransactionId;

            order.Status = OrderStatus.Confirmed;

            if (order.Shipments != null && order.Shipments.Any())
            {
                foreach (var shipment in order.Shipments)
                {
                    shipment.Status = ShipmentStatus.Shipped;
                }
            }

            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}