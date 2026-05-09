using ECommerce.Application.DTOs;
using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentResultDto> ProcessPaymentAsync(decimal amount, PaymentMethod paymentMethod, int orderId);
        Task<bool> CapturePayPalPaymentAsync(string paypalOrderId);
    }
}
