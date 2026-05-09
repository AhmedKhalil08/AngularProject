using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Payments.Queries.GetPaymentById
{
    public class GetPaymentByIdQuery: IRequest<PaymentDto>
    {
        public int Id { get; set; }
    }
}
