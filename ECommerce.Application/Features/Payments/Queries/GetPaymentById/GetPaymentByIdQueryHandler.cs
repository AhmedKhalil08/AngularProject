using ECommerce.Application.DTOs;
using ECommerce.Application.Features.OrderItems.Queries.GetAllOrderItems;
using ECommerce.Application.Interfaces.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Payments.Queries.GetPaymentById
{
    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto>
    {
        private readonly IPaymentRepository _repository;

        public GetPaymentByIdQueryHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaymentDto> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null) { return null; }
            return new PaymentDto
            {
                Id = item.Id,
                Amount = item.Amount,
                Method = item.Method,
                Status = item.Status,
                PaidAt = item.PaidAt,
                OrderId = item.OrderId
            };
        }

    }
}
