using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommandHandler: IRequestHandler<UpdatePaymentCommand, PaymentDto>
    {
        private readonly IPaymentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderCommandHandler(IOrderRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<PaymentDto> Handle(UpdatePaymentCommand request,CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) return null;

            if (item == null)
                throw new Exception("Payment not found");

            item.Status = request.Status;

            // Optional:
            if (request.Status == PaymentStatus.Paid)
            {
                item.PaidAt = DateTime.UtcNow;
              
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new PaymentDto
            {
                Id = item.Id,
                Amount = item.Amount,
                Method = item.Method,
                Status = item.Status,
                PaymentDate = item.PaymentDate,
                PaidAt = item.PaidAt,
                OrderId = item.OrderId
            };
        }
    }
}
