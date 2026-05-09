using ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string TransactionId { get; set; } = "";
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? PaidAt { get; set; }

        public int OrderId { get; set; }
    }
}
