using ECommerce.Application.DTOs;
using ECommerce.Application.Features.OrderItems.Commands.UpdateOrderItem;
using ECommerce.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler: IRequestHandler<UpdateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderCommandHandler(IOrderRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return null;
            item.Status = request.Status;
            item.TotalAmount = request.TotalAmount;
            item.Payment = new Domain.Entities.Payment
            {
                Amount = request.Payment.Amount,
                Method = (Domain.Enums.PaymentMethod)request.Payment.Method,
                Status = (Domain.Enums.PaymentStatus)request.Payment.Status,
                PaidAt = request.Payment.PaidAt
            };
            await _repository.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return new OrderDto
            {
            Id = item.Id,
            Status = item.Status,
            TotalAmount = item.TotalAmount,
            Payment = item.Payment

            };
        }
    }
}
