using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Payments.Queries.GetAllPayments
{
    internal class GetAllPaymentsQuery: IRequest<List<PaymentDto>>
    {
    }
}
