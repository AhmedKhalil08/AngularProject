using ECommerce.Application.DTOs;
using ECommerce.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Features.Payments.Commands.UpdatePayment
{
    public class ConfirmPaymentCommand : IRequest<bool>
    {
        public int OrderId { get; set; }
        public string TransactionId { get; set; } // رقم العملية اللي جيالك من Stripe
    }
}
