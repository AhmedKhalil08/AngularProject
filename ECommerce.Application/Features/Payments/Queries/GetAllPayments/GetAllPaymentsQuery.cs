using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Payments.Queries.GetAllPayments
{
    public class GetAllPaymentsQuery: IRequest<List<PaymentDto>>
    {
    }
}
