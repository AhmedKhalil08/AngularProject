using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs
{
    public class PaymentResultDto
    {
        public bool IsSuccess { get; set; }
        public string PaymentUrl { get; set; } 
      
            public string Message { get; set; }
    }
}
