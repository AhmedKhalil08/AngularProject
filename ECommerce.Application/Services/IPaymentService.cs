using ECommerce.Application.DTOs;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public interface IPaymentService
    {
        Task<PaymentResultDto> ProcessPaymentAsync(decimal amount, PaymentMethod paymentMethod);
        Task<bool> CapturePayPalPaymentAsync(string paypalOrderId);
    }
}
