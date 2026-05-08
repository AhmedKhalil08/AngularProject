using ECommerce.Application.DTOs;
using ECommerce.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommand : IRequest<PaymentDto>
    {
        public int PaymentId { get; set; }

        public PaymentStatus Status { get; set; }

    
    }
}
