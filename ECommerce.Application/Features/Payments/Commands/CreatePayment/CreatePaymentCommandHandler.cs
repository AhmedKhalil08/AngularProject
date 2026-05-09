using System;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using MediatR;


namespace ECommerce.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
    {
        private readonly IPaymentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentCommandHandler(IPaymentRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            PaymentMethod method = default;
            if (!string.IsNullOrWhiteSpace(request.PaymentMethod))
            {
                Enum.TryParse<PaymentMethod>(request.PaymentMethod, true, out method);
            }

            var payment = new Payment
            {
                OrderId = request.OrderId,
                Amount = request.Amount,

                Method = method,
                Status = PaymentStatus.Pending,
                PaidAt = DateTime.UtcNow
            };
            await _repository.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return new PaymentDto
            {
                Id = payment.Id,
                Amount = payment.Amount,
                Method = payment.Method,
                Status= payment.Status,
                PaidAt= payment.PaidAt,
                OrderId= payment.OrderId
            };
        }
    }

      
} 
