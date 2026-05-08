using ECommerce.Application.DTOs;
using ECommerce.Application.Features.OrderItems.Queries.GetAllOrderItems;
using ECommerce.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ECommerce.Application.Features.Payments.Queries.GetAllPayments
{
    public class GetAllPaymentsQueryHandler:IRequestHandler<GetAllPaymentsQuery, List<PaymentDto>>
    {
        private readonly IPaymentRepository _repository;

        public GetAllPaymentsQueryHandler(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllAsync();
            return items.Select(item => new PaymentDto
            {
                Id = item.Id,
                Amount = item.Amount,
                Method = item.Method,
                Status = item.Status,
                PaidAt = item.PaidAt,
                OrderId = item.OrderId
            }).ToList();
        }
    }
}
