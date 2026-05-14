using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class MonthlySalesDto
    {
        public string Month { get; set; }
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
    }
}
