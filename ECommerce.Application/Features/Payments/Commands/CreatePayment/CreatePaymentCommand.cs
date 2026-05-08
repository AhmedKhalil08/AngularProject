using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Payments.Commands.CreatePayment
{
    
    public class CreatePaymentCommand: IRequest<PaymentDto>
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
