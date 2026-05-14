using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.DTOs
{
    public class TopProductDto
    {
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
        public decimal Revenue { get; set; }
    }
}
